using System;

public class RegularActionController
{
    private readonly RegularActionPanel _panel;
    private HeroMoveAction _heroMoveAction;    
    
    private RegularAction _currentAction;
    private Action _complete;
    public RegularActionController(RegularActionPanel panel)
    {
        _panel = panel;        
        _panel.HidePanel();
        _panel.HeroMoveClicked += OnHeroMoveClicked;        
    }
    private void OnHeroMoveClicked()
    {
        _panel.HidePanel();
        _heroMoveAction = new();
        _heroMoveAction.Start(_currentAction.Player);        
    }

    public void StartRegularAction(RegularAction regularAction, Action onCompleted)
    {
        _currentAction = regularAction;
        _complete = onCompleted;
        _panel.ShowPanel();
        _panel.UpdateButtonInteractability(_currentAction);
    }
}
