using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (CellManager))]
public class Chloroplast : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CellManager map = target as CellManager;
    }
}
