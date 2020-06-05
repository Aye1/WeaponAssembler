using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(EquipmentLayout))]
public class EquipmentLayoutEditor : Editor
{
    private Vector2Int _selectedCellPos = new Vector2Int(-1, -1);

    private Color _selectedCellColor = new Color(0.6f, 0.6f, 0.6f);
    private Vector2Int _noSelectedCellPos = new Vector2Int(-1, -1);

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
        GenerateSelectedCellInfo();
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
                bool isSelectedCell = _selectedCellPos.x == i && _selectedCellPos.y == j;
                if (isSelectedCell)
                {
                    GUI.backgroundColor = _selectedCellColor;
                }
                if (GUILayout.Button(buttonName, options))
                {
                    if (!isSelectedCell)
                    {
                        _selectedCellPos = new Vector2Int(i, j);
                    }
                    else
                    {
                        _selectedCellPos = _noSelectedCellPos;
                    }
                }
                if(isSelectedCell)
                {
                    GUI.backgroundColor = Color.white;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorUtility.SetDirty(equip);
    }

    private void GenerateSelectedCellInfo()
    {
        EquipmentLayout equip = (EquipmentLayout)target;
        int x = _selectedCellPos.x;
        int y = _selectedCellPos.y;
        if (_selectedCellPos.x != -1)
        {
            EditorUtils.Header("Cell Info");
            CellState tmpState = (CellState)EditorGUILayout.EnumPopup("State", equip.GetState(x,y), typeEnumDropdownOptions);
            equip.SetState(x, y, tmpState);
        }
    }
}
