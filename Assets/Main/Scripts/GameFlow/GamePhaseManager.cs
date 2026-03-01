using System;
using System.Collections.Generic;
using UnityEngine;

internal class GamePhaseManager
{
    internal GamePhaseBase CurrentPhase { get; private set; }
    private readonly Dictionary<Type, GamePhaseBase> _phases = new();

    internal event Action<GamePhaseBase> OnPhaseChanged;

    internal GamePhaseManager (IReadOnlyList<Player> players) {
        _phases[typeof(GamePhaseHeroPlacement)] = new GamePhaseHeroPlacement(this, players);
        _phases[typeof(GamePhasePlayerTurn)] = new GamePhasePlayerTurn(this, players);
    }
    internal void StartHeroesPlacement() {
        SwitchPhase<GamePhaseHeroPlacement>();
    }
    internal void NextPhase(GamePhaseBase prevPhase) {
        if (prevPhase is GamePhaseHeroPlacement) {
            SwitchPhase<GamePhasePlayerTurn>();
        } else if (prevPhase is GamePhasePlayerTurn) {
            SwitchPhase<GamePhasePlayerTurn>();
        }
    }
    private void SwitchPhase<T>() where T : GamePhaseBase {
        CurrentPhase?.OnExit();
        CurrentPhase = _phases[typeof(T)];
        CurrentPhase.OnEnter();
        OnPhaseChanged?.Invoke(CurrentPhase);
    }
}
