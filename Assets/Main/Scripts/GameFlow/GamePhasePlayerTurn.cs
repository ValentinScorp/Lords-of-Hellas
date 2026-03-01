using System.Collections.Generic;
using UnityEngine;

internal class GamePhasePlayerTurn : GamePhaseBase
{
    internal override string Name => "Player Turn Phase";

    private TurnOrderManager _turnOrder;
    private PlayerTurnController _playerTurnController;

    internal GamePhasePlayerTurn( GamePhaseManager phaseManager, IReadOnlyList<Player> players)
        : base (phaseManager) {
        _playerTurnController = new PlayerTurnController();
        _turnOrder = new TurnOrderManager(players);
        _turnOrder.PlayerChanged += HandleNextPlayer;
        _turnOrder.NoPlayersLeft += ProceedNextPhase;
    }
    internal override void OnEnter() {
        _turnOrder.ResetToFirstPlayer();       
        InitPlayerTurn(_turnOrder.CurrentPlayer);
    }
    internal override void OnExit() {
       
    }    
    private void ProceedNextPhase() {        
        PhaseManager.NextPhase(this);
    }
    private void HandleNextPlayer(Player player) {
        GameContext.Instance.CurrentPlayer = player;
        InitPlayerTurn(player);
    }
    private void InitPlayerTurn(Player player) {
        _playerTurnController.Launch(player, HandleTurnCompleted);
    }
    private void HandleTurnCompleted(Player player) {
        _turnOrder.NextPlayer();
    }
}
