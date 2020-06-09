using System.Linq;
using UnityEngine;

public enum EquipmentType { Unknown, Connector, Shooter };

[CreateAssetMenu(fileName = "EquipmentLayout", menuName = "ScriptableObjects/EquipmentLayout", order = 1)]
public class EquipmentLayout : ScriptableObject
{
    [SerializeField] private CellState[] _states = { };
    [SerializeField] private Cell[] _cells = { };
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
                CellState[,] snapshot = CreateStatesSnapshot();
                Cell[,] cellSnapshot = CreateCellsSnapshot();
                _rows = value;
                ResetStates(snapshot);
                ResetCells(cellSnapshot);
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
                CellState[,] snapshot = CreateStatesSnapshot();
                Cell[,] cellSnapshot = CreateCellsSnapshot();
                _cols = value;
                ResetStates(snapshot);
                ResetCells(cellSnapshot);
            }
        }
    }

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
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

    public Cell GetCell(int x, int y)
    {
        if (x < Cols && y < Rows && x >= 0 && y >= 0 && GetState(x,y) != CellState.Empty)
        {
            return _cells[x + y * Cols];
        }
        return null;
    }

    public void SetCell(int x, int y, Cell cell)
    {
        if (x < Cols && y < Rows && x >= 0 && y >= 0 && GetState(x,y) != CellState.Empty)
        {
            _cells[x + y * Cols] = cell;
        }
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
    }

    private CellState[,] CreateStatesSnapshot()
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
    }

    public bool CheckLayout()
    {
        return _cells.Count(x => x != null) == _states.Count(x => x != CellState.Empty && x != CellState.Inactive);
    }
}
