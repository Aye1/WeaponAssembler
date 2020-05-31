using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Equipment equip = (Equipment)target;
        EditorGUILayout.Space();
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
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GenerateCellsMatrix(equip);
    }

    private void GenerateCellsMatrix(Equipment equip)
    {
        int cols = equip.Cols;
        int rows = equip.Rows;

        GUILayoutOption[] options = new GUILayoutOption[]
        {
            GUILayout.Width(30),
            GUILayout.Height(30)
        };
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        for(int j = 0; j < rows; j++)
        {
            EditorGUILayout.BeginHorizontal();
            for(int i = 0; i < cols; i++)
            {
                CellState state = equip.GetLayoutState(i, j);
                string buttonName = state.ToString().Substring(0,1);
                if(GUILayout.Button(buttonName, options))
                {
                    Debug.Log("button clicked (" + i + ", " + j + ")");
                    equip.SetState(i, j, GetNextState(state));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private CellState GetNextState(CellState state)
    {
        return (CellState)(((int)state + 1) % Enum.GetNames(typeof(CellState)).Length);
    }
}
