using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cytochrome : MonoBehaviour
{
    public CellManager CM;
    public float delay;

    void Start()
    {
         CM = CellManager.instance;
         InvokeRepeating("GenerateProtons", delay, delay);
    }

    public void GenerateProtons()
    {
        CM.Protons++;
    }
}
