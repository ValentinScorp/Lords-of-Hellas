using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePhaseManager
{
    public GamePhaseBase CurrentPhase { get; private set; }
    private readonly Dictionary<Type, GamePhaseBase> _phases = new();

    public event Action<GamePhaseBase> OnPhaseChanged;

    public GamePhaseManager (   IReadOnlyList<Player> players,
                                TokenPlacementManager placementManager,
                                TurnManager turnManager) {
        _phases[typeof(GamePhaseHeroPlacement)] = new GamePhaseHeroPlacement(this, players, placementManager);
        _phases[typeof(GamePhasePlayerTurn)] = new GamePhasePlayerTurn(this, players, turnManager);
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
        CurrentPhase?.Exit();
        CurrentPhase = _phases[typeof(T)];
        CurrentPhase.Enter();
        OnPhaseChanged?.Invoke(CurrentPhase);
    }
    public void Update() {
        CurrentPhase?.Update();
    }
}
