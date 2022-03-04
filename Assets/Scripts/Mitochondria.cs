using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mitochondria : MonoBehaviour
{
    public CellManager CM;
    public float delay;

    void Start()
    {
         CM = CellManager.instance;
         InvokeRepeating("GenerateATP", delay, delay);
    }

    public void GenerateATP()
    {
        CM.ATP++;
        Debug.Log(CM.ATP.ToString());
    }
}
