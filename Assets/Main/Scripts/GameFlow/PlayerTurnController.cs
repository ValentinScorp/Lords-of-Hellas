using System;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerTurnController
{
    private RegularActionController _regularActionController;
    private SpecialActionsController _specialActionController;

    internal Action<Player> _TurnCompleted;
    internal void Launch(Player player, Action<Player> TurnCompleted)
    {
        _TurnCompleted = TurnCompleted;

        SceneUiRegistry.Get<TurnStartPanel>().Launch(player, StartRegularAction); 
        
        SceneUiRegistry.Get<PlayerInfoUiPanel>().Bind(player); 
    }
    internal void StartRegularAction(Player player) {

        _regularActionController = new RegularActionController();
        _regularActionController.Launch(player, RegularActionComplete);
    }
    internal void StartSpecialAction(Player player) {
        _specialActionController = new SpecialActionsController();
        _specialActionController.Launch(player, SpecialActionCompleted);
    }
    private void RegularActionComplete(Player player)
    {
        StartSpecialAction(player);        
    }
    private void SpecialActionCompleted(Player player)
    {
        _TurnCompleted?.Invoke(player);
    }
}
