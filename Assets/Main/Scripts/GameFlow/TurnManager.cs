using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    private readonly IReadOnlyList<Player> _players;

    public IReadOnlyList<Player> Players => _players;

    public event Action OnTurnCompleted;
    public TurnManager(IReadOnlyList<Player> players) {
        _players = players ?? throw new ArgumentNullException(nameof(players));
    }
    public void StartRegularAction(Player player) {
        var regularActionManager = ServiceLocator.Get<RegularActionManager>();
        regularActionManager.Launch(player, RegularActionComplete);
    }
    public void StartSpecialAction(Player player) {
        
    }
    private void RegularActionComplete()
    {
        OnTurnCompleted?.Invoke();
    }
}
