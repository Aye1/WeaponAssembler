using UnityEngine;
using System.Collections;
using System;

public static class EquipmentEditorUtils
{
    public static CellState GetNextState(CellState state)
    {
        int totalStates = Enum.GetNames(typeof(CellState)).Length;
        int nextState = ((int)state + 1) % totalStates;
        // Inactive state is just used for initialization, not useful for configuration, so we skip it
        nextState = nextState == 0 ? 1 : nextState;
        return (CellState)nextState;
    }

    public static string GetCellButtonText(CellState state)
    {
        string res = "";
        switch (state)
        {
            case CellState.Empty:
                res = "";
                break;
            case CellState.Inactive:
                res = "?";
                break;
            case CellState.Open:
                res = "O";
                break;
            case CellState.Used:
                res = "X";
                break;
        }
        return res;
    }
}
