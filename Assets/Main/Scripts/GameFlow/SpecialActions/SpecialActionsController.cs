using System;
using UnityEngine;

public class SpecialActionsController
{
    private Player _player;
    private Action<Player> _Completed;
    private SpecialActionsPanel _uiPanel;

    private SpecialActionPrepareController _prepareController = new();
    private SpecialActionRecruit _recruitAction = new();
    
    internal SpecialActionsController()
    {
        _uiPanel = SceneUiRegistry.Get<SpecialActionsPanel>();
        if (_uiPanel is null) {
            Debug.LogWarning("Can't get UI Panel for Player Special Action!");            
        }
    }
    internal void Launch(Player player, Action<Player> Completed)
    {
        _player = player;
        _Completed = Completed;

        player?.ResetHoplitesMove();

        _uiPanel?.Bind(this);
        _uiPanel?.Show(true);        
    }
    internal void OnPreparePressed()
    {
        _uiPanel?.Show(false);
        _prepareController.Launch(_player, OnSpecialActionCompleted);
    }
    

    internal void OnBuildTemplePressed()
    {
        throw new NotImplementedException();
    }

    internal void OnHuntPressed()
    {
        throw new NotImplementedException();
    }

    internal void OnMarchPressed()
    {
        throw new NotImplementedException();
    }

    internal void OnRecruitPressed()
    {
        _recruitAction.Launch(_player, OnSpecialActionCompleted);
    }

    internal void OnUsurpPressed()
    {
        throw new NotImplementedException();
    }
    internal void OnBuildMonumentPressed()
    {
        OnSpecialActionCompleted(_player);
    }    
    private void OnSpecialActionCompleted(Player player)
    {
        _uiPanel?.Undbind(this);
        _uiPanel?.Show(false);
        _Completed?.Invoke(player);
    }
}
