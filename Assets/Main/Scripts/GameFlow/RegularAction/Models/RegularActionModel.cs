
using System;
using UnityEngine;

public abstract class RegularActionModel
{
    public bool CanUndo { get; private set; }
    public bool CanCancel { get; private set; }
    public bool CanDone { get; private set; }

    public event Action<bool> CanUndoChanged;
    public event Action<bool> CanCancelChanged;
    public event Action<bool> CanDoneChanged;

    public void UndoLast()
    {
        Debug.Log("Not implemented yet!");
        SetCanUndo(false);
    }
    public void UndoAll()
    {
        Debug.Log("Not implemented yet!");
        SetCanUndo(false);
    }

    protected void SetCanUndo(bool value)
    {
        if (CanUndo == value) return;
        CanUndo = value;
        CanUndoChanged?.Invoke(value);
    }
}
