using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;

[ExecuteInEditMode]
public class Equipment : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public EquipmentLayout layout;

    private Cell[] cells;
    private int cellSize = 50;

    public Cell cellTemplate;
    
    private bool dragging = false;
    private DynamicGrid grid;
    private CellState[,] _states;

    /*private Vector2Int _size;
    public Vector2Int Size
    {
        get {
            if(_size == Vector2.zero)
            {
                _size = new Vector2Int(_states.GetLength(1), _states.GetLength(0));
            }
            return _size;
        }
    }*/

    public int Cols
    {
        get { return layout.Cols; }
    }

    public int Rows
    {
        get { return layout.Rows; }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (layout != null)
        {
            if (transform.childCount == 0)
            {
                GenerateCells();
            }
            else
            {
                FindExistingCells();
            }
            grid = FindObjectOfType<DynamicGrid>();
        }
    }

    public void GenerateCells()
    {
        ClearCells();
        _states = new CellState[layout.Rows, layout.Cols];
        cells = new Cell[layout.CellCount()];
        int rows = layout.Rows;
        int cols = layout.Cols;
        int k = 0;
        Vector3 offset = new Vector3(-(cols-1)*0.5f*cellSize, (rows-1)*0.5f*cellSize, 0.0f);
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (layout.GetState(i, j) != CellState.Empty)
                {
                    Cell newCell = Instantiate(cellTemplate, transform);
                    newCell.transform.localPosition = new Vector3(i * cellSize, -j * cellSize, 0.0f) + offset;
                    newCell.state = layout.GetState(i, j);
                    SetState(i, j, newCell.state);
                    cells[k] = newCell;

                    k++;
                }
            }
        }
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(cols * cellSize, rows * cellSize);
    }

    private void FindExistingCells()
    {
        _states = new CellState[layout.Rows, layout.Cols];
        cells = new Cell[layout.CellCount()];
        int i = 0;
        foreach(Cell cell in GetComponentsInChildren<Cell>())
        {
            cells[i] = cell;
            SetState(cell.x, cell.y, cell.state);
            i++;
        }
    }

    public CellState GetLayoutState(int col, int row)
    {
        return layout.GetState(col, row);
    }

    public CellState GetState(int col, int row)
    {
        return _states[row, col];
    }

    public void SetState(int col, int row, CellState state)
    {
        _states[row, col] = state;
    }

    public CellState[,] GetAllStates()
    {
        return _states;
    }

    public IEnumerable<CellState> GetAllStatesList()
    {
        return _states.Cast<CellState>().ToList();
    }

    public void ClearCells()
    {
        List<Transform> tempList = transform.Cast<Transform>().ToList();
        foreach (Transform t in tempList)
        {
            if (t != transform)
            {
                DestroyImmediate(t.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetDraggedPosition();
        SetAlpha(dragging);
    }

    public void OnDrag(PointerEventData eventData)
    {
        grid.ManageDrag(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ?
        Debug.Log("drop");
        grid.PutEquipment(this);
    }

    private void SetDraggedPosition()
    {
        if (dragging)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
            transform.position = mousePos;
        }
    }

    private void SetAlpha(bool transp)
    {
        foreach(Cell cell in cells)
        {
            if(cell != null) 
                cell.transp = transp;
        }
    }
}
