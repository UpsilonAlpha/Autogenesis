using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATPSynthase : MonoBehaviour
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
        if (CM.Protons > 60)
        {
            CM.ATP++;
            CM.Protons -= 4;
        }
    }
}
