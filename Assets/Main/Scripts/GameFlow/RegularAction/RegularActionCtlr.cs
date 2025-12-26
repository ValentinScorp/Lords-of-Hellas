using System;
using UnityEngine;

public class RegularActionCtlr
{
    private readonly RegularActionPanel _panel;
    private HeroMoveAction _heroMoveAction;    
    private HopliteMoveAction _hopliteMoveAction;    
    
    private RegularAction _currentAction;
    private Action _complete;
    public RegularActionCtlr(RegularActionPanel panel)
    {
        _panel = panel;        
        _panel.HidePanel();
    }
    public void OnHeroMoveClicked()
    {
        _panel.HidePanel();
        _heroMoveAction = new();
        _heroMoveAction.Start(_currentAction, OnHeroMoveCompleted);        
    }
    public void OnHopliteMoveClicked()
    {
        _panel.HidePanel();
        _hopliteMoveAction = new();
        _hopliteMoveAction.Start(_currentAction, OnHopliteMoveCompleted);
    }
    public void StartRegularAction(RegularAction regularAction, Action onCompleted)
    {
        _currentAction = regularAction;
        _complete = onCompleted;
        _panel.ShowPanel();
        _panel.UpdateButtonInteractability(_currentAction);
    }
    private void OnHeroMoveCompleted()
    {
       _panel.ShowPanel();
       _panel.UpdateButtonInteractability(_currentAction);
    }
    private void OnHopliteMoveCompleted()
    {
        _panel.ShowPanel();
        _panel.UpdateButtonInteractability(_currentAction);
        Debug.Log("Hoplite move completed!");
    }
}
