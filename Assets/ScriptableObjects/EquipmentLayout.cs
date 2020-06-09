using System.Linq;
using UnityEngine;
using System;

public enum EquipmentType { Unknown, Connector, Shooter };

[Serializable]
public struct CellBinding
{
    public Cell cell;
    public CellState state;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "EquipmentLayout", menuName = "ScriptableObjects/EquipmentLayout", order = 1)]
public class EquipmentLayout : ScriptableObject
{
    //[SerializeField] private CellState[] _states = { };
    //[SerializeField] private Cell[] _cells = { };
    [SerializeField] private CellBinding[] _bindings = { };
    [SerializeField] private int _rows = 0;
    [SerializeField] private int _cols = 0;
    public EquipmentType type;

    public int Rows
    {
        get { return _rows; }
        set
        {
            if(value != _rows)
            {
                //CellState[,] snapshot = CreateStatesSnapshot();
                //Cell[,] cellSnapshot = CreateCellsSnapshot();
                CellBinding[,] bindingsSnapshot = CreateBindingsSnapshot();
                _rows = value;
                //ResetStates(snapshot);
                //ResetCells(cellSnapshot);
                ResetBindings(bindingsSnapshot);
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
                //CellState[,] snapshot = CreateStatesSnapshot();
                //Cell[,] cellSnapshot = CreateCellsSnapshot();
                CellBinding[,] bindingsSnapshot = CreateBindingsSnapshot();
                _cols = value;
                //ResetStates(snapshot);
                //ResetCells(cellSnapshot);
                ResetBindings(bindingsSnapshot);
            }
        }
    }

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public CellBinding GetBinding(int x, int y)
    {
        if (x < Cols && y < Rows && x >= 0 && y >= 0)
        {
            return _bindings[x + y * Cols];
        }
        return default;
    }

    public CellState GetState(int x, int y)
    {
        /*if(x < Cols && y < Rows && x >= 0 && y >= 0)
        {
            return _states[x + y * Cols];
        }
        return CellState.Inactive;*/
        return GetBinding(x, y).state;
    }

    public void SetState(int x, int y, CellState state)
    {
        if (x < Cols && y < Rows && x >= 0 && y >= 0)
        {
            //_states[x + y * Cols] = state;
            _bindings[x + y * Cols].state = state;
        }
    }

    public CellState[] GetAllStates()
    {
        //return _states;
        return _bindings.Select(x => x.state).ToArray();
    }

    public Cell GetCell(int x, int y)
    {
        /*if (x < Cols && y < Rows && x >= 0 && y >= 0 && GetState(x,y) != CellState.Empty)
        {
            return _cells[x + y * Cols];
        }
        return null;*/
        return GetBinding(x, y).cell;
    }

    public void SetCell(int x, int y, Cell cell)
    {
        if (x < Cols && y < Rows && x >= 0 && y >= 0 && GetState(x,y) != CellState.Empty)
        {
            //_cells[x + y * Cols] = cell;
            _bindings[GridUtils.BtoLIndex(x, y, Cols)].cell = cell;
        }
    }

    public Sprite GetSprite(int x, int y)
    {
        return GetBinding(x, y).sprite;
    }

    public void SetSprite(int x, int y, Sprite sprite)
    {
        if (x < Cols && y < Rows && x >= 0 && y >= 0 && GetState(x, y) != CellState.Empty)
        {
            _bindings[GridUtils.BtoLIndex(x, y, Cols)].sprite = sprite;
        }
    }

    public int CellCount()
    {
        //return _states.Length;
        return _bindings.Length;
    }

    public int VisibleCellCount()
    {
        return _bindings.Count(x => x.state == CellState.Open || x.state == CellState.Used);
        //return _states.Count(x => x == CellState.Open || x == CellState.Used);
    }

    /*private void ResetStates(CellState[,] snapshot)
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

    private void ResetCells(Cell[,] snapshot)
    {
        Cell[] newCells = new Cell[Rows * Cols];
        for (int i = 0; i < Mathf.Min(Cols, snapshot.GetLength(0)); i++)
        {
            for (int j = 0; j < Mathf.Min(Rows, snapshot.GetLength(1)); j++)
            {
                newCells[i + j * Cols] = snapshot[i, j];
            }
        }
        _cells = newCells;
    }*/

    private void ResetBindings(CellBinding[,] snapshot)
    {
        CellBinding[] newBindings = new CellBinding[Rows * Cols];
        for (int i = 0; i < Mathf.Min(Cols, snapshot.GetLength(0)); i++)
        {
            for (int j = 0; j < Mathf.Min(Rows, snapshot.GetLength(1)); j++)
            {
                newBindings[i + j * Cols] = snapshot[i, j];
            }
        }
        _bindings = newBindings;
    }

    private CellBinding[,] CreateBindingsSnapshot()
    {
        CellBinding[,] snapshot = new CellBinding[Cols, Rows];
        for (int i = 0; i < Cols; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                snapshot[i, j] = GetBinding(i, j);
            }
        }
        return snapshot;
    }

    /*private CellState[,] CreateStatesSnapshot()
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

    private Cell[,] CreateCellsSnapshot()
    {
        Cell[,] snapshot = new Cell[Cols, Rows];
        for (int i = 0; i < Cols; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                snapshot[i, j] = GetCell(i, j);
            }
        }
        return snapshot;
    }*/

    public bool CheckLayout()
    {
        return _bindings.Count(x => x.cell != null)
            == _bindings.Count(x => x.state != CellState.Empty && x.state != CellState.Inactive);
    }
}
