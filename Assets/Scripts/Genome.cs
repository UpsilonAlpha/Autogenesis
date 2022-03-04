using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome : MonoBehaviour
{
    CellManager CM;
    public GameObject ui;
    public List<Organelle> Organelles;
    public List<Organelle> Proteins;

    private void Start()
    {
        CM = CellManager.instance;
    }

    public void SelectToBuild(string name)
    {
        switch (name)
        {
            case "Ribosome":
                CM.SettoBuild(Proteins[0]);
                break;
            case "PSII":
                CM.SettoBuild(Proteins[1]);
                break;
            case "Cytochrome":
                CM.SettoBuild(Proteins[2]);
                break;
            case "ATPSynthase":
                CM.SettoBuild(Proteins[3]);
                break;
            case "Thylakoid":
                CM.SettoBuild(Proteins[4]);
                break;

            case "Chloroplast":
                CM.SettoBuild(Organelles[0]);
                break;
            case "Mitochondria":
                CM.SettoBuild(Organelles[1]);
                break;
        }
    }

    public void Slide()
    {
        ui.SetActive(!ui.activeSelf);
    }
}
