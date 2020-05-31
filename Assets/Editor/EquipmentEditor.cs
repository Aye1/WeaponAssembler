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
        EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);
        GenerateCellsMatrix(equip);
    }

    private void GenerateCellsMatrix(Equipment equip)
    {
        int cols = equip.Size.x;
        int rows = equip.Size.y;

        GUILayoutOption[] options = new GUILayoutOption[]
        {
            GUILayout.Width(20),
            GUILayout.Height(20)
        };
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        for(int j = 0; j < rows; j++)
        {
            EditorGUILayout.BeginHorizontal();
            for(int i = 0; i < cols; i++)
            {
                CellState state = equip.GetState(i, j);
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
