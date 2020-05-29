using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

[ExecuteInEditMode]
public class Equipment : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public CellState[,] states = {  { CellState.Empty, CellState.Used, CellState.Open },
                                    { CellState.Empty, CellState.Used, CellState.Empty },
                                    { CellState.Open, CellState.Open, CellState.Open }
                                 };

    private Cell[] cells;

    public Cell cellTemplate;
    
    private bool dragging = false;
    private DynamicGrid grid;


    // Start is called before the first frame update
    void Start()
    {
        if(transform.childCount == 0)
        {
            GenerateCells();
        } else
        {
            FindExistingCells();
        }
        grid = FindObjectOfType<DynamicGrid>();
    }

    public void GenerateCells()
    {
        ClearCells();
        cells = new Cell[9];
        int k = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (states[i + 1, j + 1] != CellState.Empty)
                {
                    Cell newCell = Instantiate(cellTemplate, transform);
                    newCell.transform.localPosition = new Vector3(i * 50, j * 50, 0.0f);
                    newCell.state = states[i + 1, j + 1];
                    cells[k] = newCell;

                    k++;
                }
            }
        }
    }

    private void FindExistingCells()
    {
        cells = new Cell[9];
        int i = 0;
        foreach(Cell cell in GetComponentsInChildren<Cell>())
        {
            cells[i] = cell;
            i++;
        }
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
