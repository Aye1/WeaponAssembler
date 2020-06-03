using UnityEngine;
using System.Collections;
using UnityEditor;

public static class EditorUtils
{
    public static void Header(string text)
    {
        EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
    }
}
