using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    private readonly IReadOnlyList<Player> _players;
    private ActionRegularManager _actionRegularManager;

    public IReadOnlyList<Player> Players => _players;

    public event Action OnTurnCompleted;
    public TurnManager(IReadOnlyList<Player> players) {
        _players = players ?? throw new ArgumentNullException(nameof(players));
        _actionRegularManager = new();
    }
    public void StartRegularAction(Player player) {
        Debug.Log("Starting regular action!");
        _actionRegularManager.Start(player);
    }
    public void StartSpecialAction(Player player) {

    }
}
