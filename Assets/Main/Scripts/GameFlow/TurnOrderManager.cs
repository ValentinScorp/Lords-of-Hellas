using System;
using System.Collections.Generic;
using UnityEngine;

internal class TurnOrderManager
{
    private readonly IReadOnlyList<Player> _players;
    private int _currentIndex = -1;
    internal Player CurrentPlayer => _players[_currentIndex];
    internal bool HasCurrentPlayer => _currentIndex >= 0 && _currentIndex < _players.Count;
    internal int RemainingPlayers => _players.Count - (_currentIndex + 1);

    internal event Action<Player> PlayerChanged;
    internal event Action NoPlayersLeft;
    internal TurnOrderManager(IReadOnlyList<Player> players) {
        if (players == null || players.Count == 0) {
            throw new System.ArgumentException("Players list cannot be null or empty.", nameof(players));
        }
        _players = players ?? throw new ArgumentNullException(nameof(players));
    }
    internal void StartPlacement() {
        _currentIndex = _players.Count - 1;
        if (HasCurrentPlayer) {
            PlayerChanged?.Invoke(CurrentPlayer);
        }
    }
    internal bool PrevPlayer() {
        if (!HasCurrentPlayer) return false;

        _currentIndex--;
        if (HasCurrentPlayer) {
            PlayerChanged?.Invoke(CurrentPlayer);
        } else {
            NoPlayersLeft?.Invoke();
        }
        return HasCurrentPlayer;
    }
    internal bool NextPlayer() {
        if (!HasCurrentPlayer) return false;

        _currentIndex++;
        if (HasCurrentPlayer) {
            PlayerChanged?.Invoke(CurrentPlayer);
        } else {
            NoPlayersLeft?.Invoke();
        }
        return HasCurrentPlayer;
    }
    internal void ResetToFirstPlayer() {
        if (_players.Count > 0) {
            _currentIndex = 0;
        }
    }
}
