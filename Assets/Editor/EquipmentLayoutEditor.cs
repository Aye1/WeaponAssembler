using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
[CustomEditor(typeof(EquipmentLayout))]
public class EquipmentLayoutEditor : Editor
{
    #region GUILayoutOptions
    private GUILayoutOption[] typeEnumDropdownOptions = new GUILayoutOption[]
    {
        GUILayout.MinWidth(100)
    };

    GUILayoutOption[] intSliderOptions = new GUILayoutOption[]
    {
        GUILayout.MinWidth(20)
    };

    #endregion

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

        EditorUtils.Header("Type");
        EditorGUI.indentLevel++;
        equip.type = (EquipmentType)EditorGUILayout.EnumPopup("Equipment Type", equip.type, typeEnumDropdownOptions);
        EditorGUI.indentLevel--;

        EditorUtils.Header("Size");
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Rows"));
        equip.Rows = EditorGUILayout.IntSlider(equip.Rows, 1, 10, intSliderOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Cols"));
        equip.Cols = EditorGUILayout.IntSlider(equip.Cols, 1, 10, intSliderOptions);
        EditorGUILayout.EndHorizontal();
        EditorGUI.indentLevel--;
    }

    private void GenerateCellsMatrix()
    {
        EquipmentLayout equip = (EquipmentLayout)target;
        int cols = equip.Cols;
        int rows = equip.Rows;

        EditorUtils.Header("Grid");
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
                string buttonName = EquipmentEditorUtils.GetCellButtonText(state);
                if (GUILayout.Button(buttonName, options))
                {
                    Debug.Log("button clicked (" + i + ", " + j + ")");
                    equip.SetState(i, j, EquipmentEditorUtils.GetNextState(state));
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorUtility.SetDirty(equip);
    }
}
