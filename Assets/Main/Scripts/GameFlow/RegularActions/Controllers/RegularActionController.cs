
using System;
using UnityEngine;

internal class RegularActionController
{
    private RegularActionPanel _uiPanel;
    private Player _player;
    private Action<Player> _completed;
    private HeroMoveActionModel _heroMoveModel;
    private HoplitesMoveActionModel _hoplitesMoveModel;
    private HeroMoveActionController _heroMoveActionController;
    private HoplitesMoveActionController _hoplitesMoveActionController;

    internal RegularActionController()
    {
        _uiPanel = SceneUiRegistry.Get<RegularActionPanel>();
    }
    internal void Launch(Player player, Action<Player> completed)
    {
        _player = player;
        _completed = completed;
        _heroMoveModel = new HeroMoveActionModel(player);
        _hoplitesMoveModel = new HoplitesMoveActionModel(player);
        player.ResetHoplitesMove();

        if (_uiPanel is not null) {
            _uiPanel.Bind(this);
            _uiPanel.Show(true);
            _uiPanel.SetHeroMoveButtonInteractable(_heroMoveModel.CanMove());
            _uiPanel.SetHoplitesMoveButtonInteractable(_hoplitesMoveModel.CanMove());
        } else {
            Debug.LogWarning("Visual Panel for Player Regular Action not set!");
        }
    }
    internal void HeroMoveStart()
    {
        _uiPanel.Show(false);
        _heroMoveActionController = new();
        _heroMoveActionController.Start(_heroMoveModel, HandleActionCompleted);
    }
    internal void HoplitesMoveStart()
    {
        _uiPanel?.Show(false);

        _hoplitesMoveActionController = new();
        _hoplitesMoveActionController.Start(_hoplitesMoveModel, HandleActionCompleted);
    }
    internal void ArtifactsUseAction()
    {
        
    }
    internal void PrayerAction()
    {
        
    }
    internal void OnCompleteAction()
    {
        if (_uiPanel is not null) {
            _uiPanel.Undbind(this);
            _uiPanel.Show(false);
        }
        _completed?.Invoke(_player);
    }
    private void HandleActionCompleted(RegularActionType type)
    {
        if (_uiPanel is not null) {
            switch (type) {
                case RegularActionType.HopliesMove:
                    _uiPanel.SetHoplitesMoveButtonInteractable(false);
                    break;
                case RegularActionType.HeroMove:
                    _uiPanel.SetHeroMoveButtonInteractable(false);
                    break;
                default:
                    Debug.LogWarning("Unknown regular acion Type!");
                    break;
            }
            _uiPanel.Show(true);
        } 
    }
}
 