using System;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderManager
{
    private readonly IReadOnlyList<Player> _players;
    private int _currentIndex = -1;
    public Player CurrentPlayer => _players[_currentIndex];
    public bool HasCurrentPlayer => _currentIndex >= 0 && _currentIndex < _players.Count;
    public int RemainingPlayers => _players.Count - (_currentIndex + 1);

    public event Action<Player> OnPlayerChanged;
    public event Action OnNoPlayersLeft;
    public TurnOrderManager(IReadOnlyList<Player> players) {
        if (players == null || players.Count == 0) {
            throw new System.ArgumentException("Players list cannot be null or empty.", nameof(players));
        }
        _players = players ?? throw new ArgumentNullException(nameof(players));
    }
    public void StartPlacement() {
        _currentIndex = _players.Count - 1;
        if (HasCurrentPlayer) {
            OnPlayerChanged?.Invoke(CurrentPlayer);
        }
    }
    public bool PrevPlayer() {
        if (!HasCurrentPlayer) return false;

        _currentIndex--;
        if (HasCurrentPlayer) {
            OnPlayerChanged?.Invoke(CurrentPlayer);
        } else {
            OnNoPlayersLeft?.Invoke();
        }
        return HasCurrentPlayer;
    }
    public bool NextPlayer() {
        if (!HasCurrentPlayer) return false;

        _currentIndex++;
        if (HasCurrentPlayer) {
            OnPlayerChanged?.Invoke(CurrentPlayer);
        } else {
            OnNoPlayersLeft?.Invoke();
        }
        return HasCurrentPlayer;
    }
    public void ResetToFirstPlayer() {
        if (_players.Count > 0) {
            _currentIndex = 0;
        }
    }
}
