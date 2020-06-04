using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[ExecuteInEditMode]
public class DynamicGrid : MonoBehaviour
{
    public Cell cellTemplate;
    public Cell[,] gameMatrix;
    public int cellSize = 50;

    public int rowCount;
    public int columnCount;

    private List<Cell> hoveredCells;

    /*
     *  0,0            c-1,0
     * 
     * 
     * 
     * 
     *  0,r-1          c-1,r-1
     */

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount == 0)
        {
            InitMatrix();
        }
        else
        {
            FindExistingCells();
        }
        // Set one Open cell to start
        SetCellState(columnCount / 2, rowCount - 1, CellState.Open);
    }

    public void InitMatrix()
    {
        ClearGrid();
        gameMatrix = new Cell[columnCount, rowCount];
        Vector2 size = new Vector2(columnCount * cellSize, rowCount * cellSize);
        for (int i = 0; i < columnCount; i++)
        {
            for (int j = 0; j < rowCount; j++)
            {
                gameMatrix[i, j] = Instantiate(cellTemplate, transform);
                gameMatrix[i, j].transform.localPosition = new Vector3(i * cellSize + (cellSize - size.x) * 0.5f, -j * cellSize - (cellSize - size.y) * 0.5f, 0.0f);
                gameMatrix[i, j].x = i;
                gameMatrix[i, j].y = j;
            }
        }
        transform.position = new Vector3(transform.position.x - columnCount / 2 * cellSize
            , transform.position.y - rowCount / 2 * cellSize
            , transform.position.z);
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = size;
        transform.localPosition = new Vector3(0.0f, 0.0f, transform.localPosition.z);
    }

    private void FindExistingCells()
    {
        gameMatrix = new Cell[columnCount, rowCount];
        foreach (Cell cell in GetComponentsInChildren<Cell>())
        {
            gameMatrix[cell.x, cell.y] = cell;
        }
    }

    public void ClearGrid()
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

    public void SetCellState(int x, int y, CellState state)
    {
        gameMatrix[x, y].state = state;
    }

    public Cell GetClosestCell(Vector3 pos)
    {
        float minDistance = float.MaxValue;
        Cell closestCell = null;
        Vector2 refPos = new Vector2(pos.x, pos.y);
        foreach (Cell cell in gameMatrix)
        {
            Vector2 posXY = new Vector2(cell.transform.position.x, cell.transform.position.y);
            float dist = Vector2.Distance(posXY, refPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestCell = cell;
            }
        }
        return closestCell;
    }

    private bool CanPutEquipmentOnCell(EquipmentVisual e, Cell c)
    {
        List<Cell> gridCells = FindCellsWithPatternAndCenter(e, c.x, c.y);
        bool canPut = e.GetAllStates().Count(st => st != CellState.Empty) == gridCells.Count;
        foreach (Cell currCell in gridCells)
        {
            canPut = canPut && currCell.tempState == TempCellState.OK;
        }
        return canPut;
    }

    public void ManageDrag(EquipmentVisual e)
    {
        Cell closestCell = GetClosestCell(Input.mousePosition);
        hoveredCells = FindCellsWithPatternAndCenter(e, closestCell.x, closestCell.y);
        ValidateEquipmentPosition(e, hoveredCells);
    }

    public List<Cell> FindCellsWithPatternAndCenter(EquipmentVisual e, int x, int y)
    {
        ResetTempStates();
        List<Cell> subset = new List<Cell>();
        int cols = e.Cols;
        int rows = e.Rows;
        int offsetX = cols / 2;
        int offsetY = rows / 2;

        for (int i = 0; i < e.Cols; i++)
        {
            for (int j = 0; j < e.Rows; j++)
            {
                int idx = x + i - offsetX;
                int idy = y + j - offsetY;
                if (GridContains(idx, idy))
                {
                    Cell gridCell = gameMatrix[idx, idy];
                    CellState equipCellState = e.GetLayoutState(i, j);

                    if (idx <= columnCount && idx >= 0 && idy <= rowCount && idy >= 0
                    && equipCellState != CellState.Empty)
                    {
                        subset.Add(gridCell);
                        if (equipCellState == CellState.Used)
                        {
                            gridCell.tempState = gridCell.state == CellState.Open ? TempCellState.OK : TempCellState.NOK;
                        }
                        if (equipCellState == CellState.Open)
                        {
                            if (gridCell.state == CellState.Open || gridCell.state == CellState.Inactive)
                            {
                                gridCell.tempState = TempCellState.OK;
                            }
                            else
                            {
                                gridCell.tempState = TempCellState.NOK;
                            }
                        }
                    }
                }
            }
        }
        return subset;
    }

    private bool GridContains(int x, int y)
    {
        return x >= 0 && x < gameMatrix.GetLength(0) && y >= 0 && y < gameMatrix.GetLength(1);
    }

    public bool ValidateEquipmentPosition(EquipmentVisual e, List<Cell> gridCells)
    {
        List<CellState> stateList = new List<CellState>();
        foreach (CellState c in e.GetAllStates())
        {
            stateList.Add(c);
        }
        int expectedCellsCount = stateList.Count(c => c != CellState.Inactive);
        return gridCells.Count == expectedCellsCount;
    }

    private void ResetTempStates()
    {
        foreach (Cell c in gameMatrix)
        {
            c.tempState = TempCellState.NAN;
        }
    }

    public void PutEquipment(EquipmentVisual e)
    {
        Cell closestCell = GetClosestCell(Input.mousePosition);
        ManageStatesAfterPut(e, closestCell);
    }

    private void ManageStatesAfterPut(EquipmentVisual e, Cell cell)
    {
        if (CanPutEquipmentOnCell(e, cell))
        {
            int x = cell.x;
            int y = cell.y;
            int cols = e.Cols;
            int rows = e.Rows;
            int offsetX = cols / 2;
            int offsetY = rows / 2;
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Cell gridCell = gameMatrix[x + i - offsetX, y + j - offsetY];
                    gridCell.tempState = TempCellState.NAN;
                    CellState equipCellState = CellState.Empty;
                    equipCellState = e.GetLayoutState(i, j);

                    if (x + i <= columnCount && x + i >= 0 && y + j <= rowCount && y + j >= 0
                    && equipCellState != CellState.Empty)
                    {
                        if (equipCellState == CellState.Used)
                        {
                            gridCell.state = CellState.Used;
                        }
                        if (equipCellState == CellState.Open && gridCell.state == CellState.Inactive)
                        {
                            gridCell.state = CellState.Open;
                        }
                    }
                }
            }
        }
    }
}
