using System;

public class RegularActionController
{
    private readonly RegularActionPanel _panel;
    private RegularAction _currentAction;
    private Action _complete;
    public RegularActionController(RegularActionPanel panel)
    {
        _panel = panel;
        _panel.HidePanel();
    }

    public void StartRegularAction(RegularAction regularAction, Action onCompleted)
    {
        _currentAction = regularAction;
        _complete = onCompleted;
        _panel.ShowPanel();
        _panel.UpdateButtonInteractability(_currentAction);
    }
}
