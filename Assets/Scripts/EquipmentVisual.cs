using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System;

[ExecuteInEditMode]
public class EquipmentVisual : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private EquipmentLayout _layout;

    // Cells here are just for display, we could use something else if needed
    private Cell[] cells;
    private int cellSize = 50;

    public Cell cellTemplate;
    
    private bool dragging = false;
    private DynamicGrid grid;

    public int Cols
    {
        get { return Layout.Cols; }
    }

    public int Rows
    {
        get { return Layout.Rows; }
    }

    public EquipmentLayout Layout
    {
        get { return _layout; }
        set
        {
            if(_layout != value)
            {
                _layout = value;
                GenerateCells();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<DynamicGrid>();
        if (Layout != null)
        {
            if (transform.childCount == 0)
            {
                GenerateCells();
            }
            else
            {
                FindExistingCells();
            }
        }
    }

    public void GenerateCells()
    {
        ClearCells();
        cells = new Cell[Layout.VisibleCellCount()];
        int rows = Layout.Rows;
        int cols = Layout.Cols;
        Vector3 offset = new Vector3(-(cols-1)*0.5f*cellSize, (rows-1)*0.5f*cellSize, 0.0f);
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (Layout.GetState(i, j) != CellState.Empty)
                {
                    Cell newCell = Instantiate(cellTemplate, transform);
                    newCell.transform.localPosition = new Vector3(i * cellSize, -j * cellSize, 0.0f) + offset;
                    newCell.state = Layout.GetState(i, j);
                    newCell.x = i;
                    newCell.y = j;
                    cells[GridUtils.BtoLIndex(i,j,Cols)] = newCell;
                }
            }
        }
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(cols * cellSize, rows * cellSize);
    }

    private void FindExistingCells()
    {
        cells = new Cell[Layout.VisibleCellCount()];
        int i = 0;
        foreach(Cell cell in GetComponentsInChildren<Cell>())
        {
            cells[i] = cell;
            i++;
        }
    }

    public CellState GetLayoutState(int col, int row)
    {
        return Layout.GetState(col, row);
    }

    public IEnumerable<CellState> GetAllStates()
    {
        return Layout.GetAllStates().ToList();
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
        if (cells != null)
        {
            foreach (Cell cell in cells)
            {
                if (cell != null)
                    cell.transp = transp;
            }
        }
    }
}
