
using System;

public class HopliteMoveAction
{
    private RegularAction _regularAction;
    private Action _onComplete;
    public void Start(RegularAction regularAction, Action onComplete = null)
    {
        _regularAction = regularAction;
        _onComplete = onComplete;
    }
}
