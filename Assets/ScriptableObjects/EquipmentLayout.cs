using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentLayout", menuName = "ScriptableObjects/EquipmentLayout", order = 1)]
public class EquipmentLayout : ScriptableObject
{
    private CellState[,] _states = { { CellState.Open, CellState.Open, CellState.Open},
                                     { CellState.Open, CellState.Open, CellState.Open },
                                     { CellState.Open, CellState.Open, CellState.Open },
                                  };

    private int _rows = 3;
    private int _cols = 3;

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
        if(x < _states.GetLength(1) && y < _states.GetLength(0) && x >= 0 && y >= 0)
        {
            return _states[y, x];
        }
        return CellState.Inactive;
    }

    public void SetState(int x, int y, CellState state)
    {
        _states[y, x] = state;
    }

    public int CellCount()
    {
        return _states.Length;
    }

    public void ResetStates()
    {
        CellState[,] newStates = new CellState[Rows, Cols];
        for(int i=0; i<Mathf.Min(_states.GetLength(1), Cols); i++)
        {
            for(int j=0; j<Mathf.Min(_states.GetLength(0), Rows); j++)
            {
                newStates[j, i] = _states[j, i];
            }
        }
        _states = newStates;
    }
}
