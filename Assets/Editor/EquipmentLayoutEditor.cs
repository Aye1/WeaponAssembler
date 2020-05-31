using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
[CustomEditor(typeof(EquipmentLayout))]
public class EquipmentLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EquipmentLayout equip = (EquipmentLayout)target;
        GeneratePropertyFields();
        EditorGUILayout.Space();
        GenerateCellsMatrix();
    }

    private void GeneratePropertyFields()
    {
        EquipmentLayout equip = (EquipmentLayout)target;

        EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUI.indentLevel++;
        GUILayoutOption[] options = new GUILayoutOption[]
        {
            GUILayout.MinWidth(20)
        };
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Rows"));
        equip.Rows = EditorGUILayout.IntSlider(equip.Rows, 1, 10, options);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Cols"));
        equip.Cols = EditorGUILayout.IntSlider(equip.Cols, 1, 10, options);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel--;
    }

    private void GenerateCellsMatrix()
    {
        EquipmentLayout equip = (EquipmentLayout)target;
        int cols = equip.Cols;
        int rows = equip.Rows;

        EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayoutOption[] options = new GUILayoutOption[]
        {
            GUILayout.Width(30),
            GUILayout.Height(30)
        };
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        for (int j = 0; j < rows; j++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < cols; i++)
            {
                CellState state = equip.GetState(i, j);
                string buttonName = state.ToString().Substring(0, 1);
                if (GUILayout.Button(buttonName, options))
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
