using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using TMPro;

public class CellManager : MonoBehaviour
{
    public static CellManager instance;
    public Genome genome;
    public GameObject temp;
    public TextMeshProUGUI ATPText;
    public TextMeshProUGUI ProtonText;
    private Organelle toBuild;

    public bool CanBuild { get { return toBuild != null; } }
    private float atp;
    private float proteins;
    private float protons;

    private Vector3[] adjacentDirections = new Vector3[] { (Vector3)MathHelpers.DegreeToVector2(-120), (Vector3)Vector2.left , (Vector3)MathHelpers.DegreeToVector2(120) , (Vector3)MathHelpers.DegreeToVector2(60), (Vector3)Vector2.right, (Vector3)MathHelpers.DegreeToVector2(-60), new Vector3(0, 0, 0)};
    private Dictionary<Vector3Int, TileBase> neighbours;
    private Grid grid;

    public Tilemap cytoplasm = null;
    public Tilemap overlay = null;
    public TileBase hoverTile = null;
    public TileBase baseTile = null;
    public TileBase builtTile = null;
    public TileBase thylakoidTile = null;

    private Vector3Int mousePos = new Vector3Int();
    private Vector3Int previousMousePos = new Vector3Int();
    private float size;

    public float ATP
    {
        get {return atp;}
        set
        {
            atp = value;
            ATPText.text = "ATP: " + atp.ToString();
        }
    }
    public float Protons
    {
        get {return protons;}
        set
        {
            protons = value;
            ProtonText.text = "Protons: " + protons.ToString();
        }
    }
    public float Proteins;
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
        grid = gameObject.GetComponent<Grid>();
    }

    void Update() 
    {
        if (CanBuild)
        {
            FollowCursor(temp);
        }  

        mousePos = GetMousePosition();
        if (!mousePos.Equals(previousMousePos) && cytoplasm.GetTile(mousePos) != null)
        {
            Debug.Log(mousePos);
            SetNeighbours(GetNeighbours(previousMousePos, size, overlay), null, overlay);
            SetNeighbours(GetNeighbours(mousePos, size, overlay), hoverTile, overlay);
            neighbours = GetNeighbours(mousePos, size, cytoplasm);
            previousMousePos = mousePos;
        }

        if(GetNeighbours(mousePos, size, cytoplasm).ContainsValue(builtTile))
        {
            temp.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
        }
        else
        {
            temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }

        if (Input.GetMouseButton(0))
        {
            switch (toBuild.prefab.name)
            {
                case "Thylakoid":
                    BuildOn(baseTile, thylakoidTile);
                    break;
                case "PSII":
                case "Cytochrome":
                case "ATPSynthase":
                    BuildOn(thylakoidTile, builtTile);
                    break;
                default:
                    BuildOn(baseTile, builtTile);
                    break;
            }
        }

    }
    
    public void BuildOn(TileBase checkTile, TileBase buildTile)
    {
        if (!CanBuild)
            return;
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (cytoplasm.GetTile(mousePos) == null || neighbours.ContainsValue(builtTile))
            return;
        if (!IsHomogenous(neighbours) || !neighbours.ContainsValue(checkTile))
            return;
            
        GameObject organelle = Instantiate(toBuild.prefab, grid.CellToWorld(mousePos), Quaternion.identity);
        organelle.GetComponent<SpriteRenderer>().sortingOrder = -mousePos.y;
        SetNeighbours(neighbours, buildTile, cytoplasm);
    }

    private void SetNeighbours(Dictionary<Vector3Int, TileBase> neighbours, TileBase tile, Tilemap map)
    {
        foreach (KeyValuePair<Vector3Int, TileBase> neighbour in neighbours)
        {
            map.SetTile(neighbour.Key, tile);
        }
    }

    private Dictionary<Vector3Int, TileBase> GetNeighbours(Vector3Int pos, float size, Tilemap map)
    {
        neighbours = new Dictionary<Vector3Int, TileBase>();
        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            Vector3Int neighbourPos = cytoplasm.WorldToCell(cytoplasm.CellToWorld(pos) + (Vector3)(adjacentDirections[i] * new Vector2(size, size)));
            TileBase neighbour = cytoplasm.GetTile(neighbourPos);
            if (neighbour != null && !neighbours.ContainsKey(neighbourPos))
            {
                neighbours.Add(neighbourPos, neighbour);
            }
        }
        return neighbours;
    }

    Vector3Int GetMousePosition() 
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
    public void FollowCursor(GameObject temp)
    {
        temp.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SpriteRenderer render = temp.GetComponent<SpriteRenderer>().sortingOrder = 100;
    }

    public Organelle GettoBuild()
    {
        return toBuild;
    }
    public async void SettoBuild(Organelle organelle)
    {
        toBuild = organelle;
        SetNeighbours(GetNeighbours(previousMousePos, size, overlay), null, overlay);
        size = organelle.size;
        genome.Slide();
        Destroy(temp);
        temp = Instantiate(toBuild.prefab);
    }

    public bool IsHomogenous(Dictionary<Vector3Int, TileBase> neighbours)
    {
        TileBase firstNeighbour = neighbours.ElementAt(0).Value;
        foreach (KeyValuePair<Vector3Int, TileBase> neighbour in neighbours)
        {
            if(neighbour.Value != firstNeighbour)
            {
                return false;
            }
        }
        return true;
    }
}