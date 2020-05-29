using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Equipment equip = (Equipment)target;
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Generate Cells"))
        {
            equip.GenerateCells();
        }
        if(GUILayout.Button("Clear Cells"))
        {
            equip.ClearCells();
        }
        EditorGUILayout.EndHorizontal();
    }
}
