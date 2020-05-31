using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[Serializable]
public enum CellState { Inactive, Open, Used, Empty };
public enum TempCellState { OK, NOK, NAN};

public class Cell : MonoBehaviour
{
    public int value;
    public CellState state = CellState.Inactive;
    public TempCellState tempState = TempCellState.NAN;
    public bool transp = false;
    public int x;
    public int y;

    private Text _text;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponentInChildren<Text>();
        _image = GetComponent<Image>();
        _text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        _text.text = "(" + x + ", " + y+")";
        if (tempState == TempCellState.NAN)
        {
            switch (state)
            {
                case CellState.Inactive:
                    //_text.text = "Inactive";
                    _image.color = transp ? ColorManager.Instance.cellInactiveTranspColor : ColorManager.Instance.cellInactiveColor;

                    break;
                case CellState.Open:
                    //_text.text = "Open";
                    _image.color = transp ? ColorManager.Instance.cellOpenTranspColor : ColorManager.Instance.cellOpenColor;
                    break;
                case CellState.Used:
                    //_text.text = "Used";
                    _image.color = transp ? ColorManager.Instance.cellUsedTranspColor : ColorManager.Instance.cellUsedColor;
                    break;
            }
        }
        else if (tempState == TempCellState.OK)
        {
            _image.color = ColorManager.Instance.cellOK;
        } 
        else if (tempState == TempCellState.NOK)
        {
            _image.color = ColorManager.Instance.cellNotOK;
        }
    }
}
