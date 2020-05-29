using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public readonly Color cellInactiveColor = Color.grey;
    public readonly Color cellOpenColor = Color.white;
    public readonly Color cellUsedColor = Color.cyan;
    public readonly Color cellNotOK = Color.red;
    public readonly Color cellOK = Color.green;

    [HideInInspector] public Color cellInactiveTranspColor;
    [HideInInspector] public Color cellOpenTranspColor;
    [HideInInspector] public Color cellUsedTranspColor;

    public float transpAlpha = 0.5F;

    private static ColorManager _instance;

    public static ColorManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetTranspColors();
    }

    void SetTranspColors()
    {
        cellInactiveTranspColor = new Color(cellInactiveColor.r, cellInactiveColor.g, cellInactiveColor.b, transpAlpha);
        cellOpenTranspColor = new Color(cellOpenColor.r, cellOpenColor.g, cellOpenColor.b, transpAlpha);
        cellUsedTranspColor = new Color(cellUsedColor.r, cellUsedColor.g, cellUsedColor.b, transpAlpha);
    }
}
