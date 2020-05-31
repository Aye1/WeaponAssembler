using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentLayout", menuName = "ScriptableObjects/EquipmentLayout", order = 1)]
public class EquipmentLayout : ScriptableObject
{
    private CellState[] _states = { };

    private int _rows = 0;
    private int _cols = 0;

    public int Rows
    {
        get { return _rows; }
        set
        {
            if(value != _rows)
            {
                _rows = value;
                ResetStates();
            }
        }
    }

    public int Cols
    {
        get { return _cols; }
        set
        {
            if (value != _cols)
            {
                _cols = value;
                ResetStates();
            }
        }
    }

    public CellState GetState(int x, int y)
    {
        if(x < Cols && y < Rows && x >= 0 && y >= 0)
        {
            return _states[x + y * Cols];
        }
        return CellState.Inactive;
    }

    public void SetState(int x, int y, CellState state)
    {
        if (x < Cols && y < Rows && x >= 0 && y >= 0)
        {
            _states[x + y * Cols] = state;
        }
    }

    public int CellCount()
    {
        return _states.Length;
    }

    public void ResetStates()
    {
        CellState[] newStates = new CellState[Rows*Cols];
        for(int i=0; i<Mathf.Min(_states.Length, newStates.Length); i++)
        {
            newStates[i] = _states[i];
        }
        _states = newStates;
    }
}
