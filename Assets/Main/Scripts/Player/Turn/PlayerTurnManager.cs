using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnManager
{
    private readonly IReadOnlyList<Player> _players;
    private RegularActionManager _regularActionManager;

    public IReadOnlyList<Player> Players => _players;

    public event Action OnTurnCompleted;
    public PlayerTurnManager(IReadOnlyList<Player> players) {
        _players = players ?? throw new ArgumentNullException(nameof(players));
        _regularActionManager = new();
    }
    public void StartRegularAction(Player player) {
        Debug.Log("Starting regular action!");
        _regularActionManager.Start(player);
    }
    public void StartSpecialAction(Player player) {

    }
}
