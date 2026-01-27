
using System;
using UnityEngine;

public class RegularActionManager
{
    private RegularActionPanel _panel;
    private Action _completed;
    private HeroMoveActionModel _heroMoveModel;
    private HeroMoveActionController _heroMoveActionController;

    public void Bind(RegularActionPanel panel)
    {
        _panel = panel;
    }
    public void Launch(Player player, Action completed)
    {
        _completed = completed;
        _heroMoveModel = new HeroMoveActionModel(player);

        if (_panel is not null) {
            _panel.Show(true);
            _panel.SetHeroMoveButtonInteractable(_heroMoveModel.CanMove());
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
        
    }
    private void HandleActionCompleted()
    {
        if (_panel is not null) {
            _panel.Show(true);
            _panel.SetHeroMoveButtonInteractable(_heroMoveModel.CanMove());
        } 
    }

}
 