using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

[Serializable]
public enum CellState { Inactive, Open, Used, Empty };
public enum TempCellState { OK, NOK, NAN};

public class Cell : MonoBehaviour
{
    public int value;
    public CellState state = CellState.Inactive;
    public TempCellState tempState = TempCellState.NAN;
    public int x;
    public int y;

    private Text _text;
    protected Image _image;

    void Awake()
    {
        _text = GetComponentInChildren<Text>();
        _image = GetComponent<Image>();
        //_text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        ManageColor();
    }

    private void ManageColor()
    {
        Color baseColor = Color.white;
        //_text.text = "(" + x + ", " + y+")";
        if (tempState == TempCellState.NAN)
        {
            switch (state)
            {
                case CellState.Inactive:
                    baseColor = ColorManager.Instance.cellInactiveColor;
                    break;
                case CellState.Open:
                    baseColor = ColorManager.Instance.cellOpenColor;
                    break;
                case CellState.Used:
                    baseColor = ColorManager.Instance.cellUsedColor;
                    break;
            }
        }
        else if (tempState == TempCellState.OK)
        {
            baseColor = ColorManager.Instance.cellOK;
        }
        else if (tempState == TempCellState.NOK)
        {
            baseColor = ColorManager.Instance.cellNotOK;
        }
        // Alpha is managed separately
        _image.color = new Color(baseColor.r, baseColor.g, baseColor.b, _image.color.a);
    }

    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetAlpha(float alpha)
    {
        Color newColor = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
        _image.color = newColor;
    }

    public float GetAplha()
    {
        return _image.color.a;
    }
}
