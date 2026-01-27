using System.Collections.Generic;
using UnityEngine;

public class GamePhasePlayerTurn : GamePhaseBase
{
    public override string Name => "Player Turn Phase";

    private TurnOrderManager TurnOrder;
    private TurnManager TurnManager;

    public GamePhasePlayerTurn( GamePhaseManager phaseManager, 
                                IReadOnlyList<Player> players, 
                                TurnManager turnManager)
        : base (phaseManager) {
        TurnManager = turnManager;
        TurnOrder = new TurnOrderManager(players);
        TurnOrder.PlayerChanged += HandleNextPlayer;
        TurnOrder.NoPlayersLeft += ProceedNextPhase;
    }
    public override void Enter() {
        TurnOrder.ResetToFirstPlayer();
        TurnManager.OnTurnCompleted += HandleTurnCompleted;
        InitPlayerTurn(TurnOrder.CurrentPlayer);
    }
    public override void Exit() {
        TurnManager.OnTurnCompleted -= HandleTurnCompleted;
    }
    private void HandleTurnCompleted() {
        TurnOrder.NextPlayer();
    }
    private void ProceedNextPhase() {        
        PhaseManager.NextPhase(this);
    }
    private void HandleNextPlayer(Player player) {
        GameContext.Instance.CurrentPlayer = player;
        InitPlayerTurn(player);
    }
    private void InitPlayerTurn(Player player) {
        TurnManager.StartRegularAction(player);
    }
}
