using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicGrid))]
public class DynamicGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DynamicGrid grid = (DynamicGrid)target;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Grid"))
        {
            grid.InitMatrix();
        }
        if (GUILayout.Button("Clear Grid"))
        {
            grid.ClearGrid();
        }
        EditorGUILayout.EndHorizontal();
    }
}
