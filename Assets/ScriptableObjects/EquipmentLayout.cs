using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EquipmentLayout", menuName = "ScriptableObjects/EquipmentLayout", order = 1)]
public class EquipmentLayout : ScriptableObject
{
    [SerializeField] private CellState[] _states = { };
    [SerializeField] private int _rows = 0;
    [SerializeField] private int _cols = 0;

    public int Rows
    {
        get { return _rows; }
        set
        {
            if(value != _rows)
            {
                CellState[,] snapshot = CreateCellsSnapshot();
                _rows = value;
                ResetStates(snapshot);
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
                CellState[,] snapshot = CreateCellsSnapshot();
                _cols = value;
                ResetStates(snapshot);
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

    public CellState[] GetAllStates()
    {
        return _states;
    }

    public int CellCount()
    {
        return _states.Length;
    }

    public int VisibleCellCount()
    {
        return _states.Count(x => x == CellState.Open || x == CellState.Used);
    }

    private void ResetStates(CellState[,] snapshot)
    {
        CellState[] newStates = new CellState[Rows*Cols];
        for (int i = 0; i < Mathf.Min(Cols, snapshot.GetLength(0)); i++)
        {
            for (int j = 0; j < Mathf.Min(Rows, snapshot.GetLength(1)) ; j++)
            {
                newStates[i + j * Cols] = snapshot[i,j];
            }
        }
        _states = newStates;
    }

    private CellState[,] CreateCellsSnapshot()
    {
        CellState[,] snapshot = new CellState[Cols, Rows];
        for (int i = 0; i < Cols; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                snapshot[i, j] = GetState(i, j);
            }
        }
        return snapshot;
    }
}
