using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ribosome : MonoBehaviour
{
    public CellManager CM;
    public float delay;

    void Start()
    {
        CM = CellManager.instance;
    }
}
