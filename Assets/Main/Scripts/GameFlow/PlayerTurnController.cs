using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnController
{
    private RegularActionController _regularActionController;
    private SpecialActionsController _specialActionController;

    public Action<Player> _TurnCompleted;
    public void StartRegularAction(Player player, Action<Player> TurnCompleted) {
        _TurnCompleted = TurnCompleted;
        _regularActionController = new RegularActionController();
        _regularActionController.Launch(player, RegularActionComplete);
    }
    public void StartSpecialAction(Player player) {
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
