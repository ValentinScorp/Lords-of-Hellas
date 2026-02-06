
using System;
using UnityEngine;

public class RegularActionManager
{
    private RegularActionPanel _panel;
    private Action _completed;
    private HeroMoveActionModel _heroMoveModel;
    private HoplitesMoveActionModel _hoplitesMoveModel;
    private HeroMoveActionController _heroMoveActionController;
    private HoplitesMoveActionController _hoplitesMoveActionController;

    public void Bind(RegularActionPanel panel)
    {
        _panel = panel;
    }
    public void Launch(Player player, Action completed)
    {
        _completed = completed;
        _heroMoveModel = new HeroMoveActionModel(player);
        _hoplitesMoveModel = new HoplitesMoveActionModel(player);
        player.ResetHoplitesMove();
        if (_panel is not null) {
            _panel.Show(true);
            _panel.SetHeroMoveButtonInteractable(_heroMoveModel.CanMove());
            _panel.SetHoplitesMoveButtonInteractable(_hoplitesMoveModel.CanMove());
        } else {
            Debug.LogWarning("Visual Panet for Player Regular Action not set!");
        }
    }

    public void HeroMoveStart()
    {
        if (_panel is not null) {
            _panel.Show(false);
        } 
        _heroMoveActionController = new();
        _heroMoveActionController.Start(_heroMoveModel, HandleActionCompleted);
    }
    public void HopliteMoveStart()
    {
        if (_panel is not null) {
            _panel.Show(false);
        } 
        _hoplitesMoveActionController = new();
        _hoplitesMoveActionController.Start(_hoplitesMoveModel, HandleActionCompleted);
    }
    public void UseArtifactAction()
    {
        
    }
    public void PrayerAction()
    {
        
    }
    public void CompleteAction()
    {
        _completed?.Invoke();
    }
    private void HandleActionCompleted()
    {
        if (_panel is not null) {
            _panel.Show(true);
            _panel.SetHeroMoveButtonInteractable(_heroMoveModel.CanMove());
            _panel.SetHoplitesMoveButtonInteractable(_hoplitesMoveModel.CanMove());
        } 
    }

}
 