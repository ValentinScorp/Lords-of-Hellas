using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePhaseManager
{
    public GamePhaseBase CurrentPhase { get; private set; }
    private readonly Dictionary<Type, GamePhaseBase> _phases = new();

    public event Action<GamePhaseBase> OnPhaseChanged;

    public GamePhaseManager (IReadOnlyList<Player> players) {
        _phases[typeof(GamePhaseHeroPlacement)] = new GamePhaseHeroPlacement(this, players);
        _phases[typeof(GamePhasePlayerTurn)] = new GamePhasePlayerTurn(this, players);
    }
    public void StartHeroesPlacement() {
        SwitchPhase<GamePhaseHeroPlacement>();
    }
    public void NextPhase(GamePhaseBase prevPhase) {
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
