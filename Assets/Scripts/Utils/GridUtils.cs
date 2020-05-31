using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils
{
    public static int BidimensionalToLinearIndex(int col, int row, int totalCols)
    {
        return col + row * totalCols;
    }

    public static int BtoLIndex(int col, int row, int totalCols)
    {
        return BidimensionalToLinearIndex(col, row, totalCols);
    }

    public static Vector2 LinearToBidimensionalIndex(int index, int totalCols)
    {
        return new Vector2(index % totalCols, index / totalCols);
    }

    public static Vector2 LtoBIndex(int index, int totalCols)
    {
        return LinearToBidimensionalIndex(index, totalCols);
    }
}
