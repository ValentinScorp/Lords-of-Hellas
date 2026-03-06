using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
internal class GameManager
{
    private PlayerTurnController _playerTurn;
    internal GamePhaseManager GamePhaseManager;
    
    private CardSelectPanel _cardSelectPanel;
    internal event Action OnGameStarted;

    internal GameManager(CardSelectPanel cardSelectPanel) {
        _cardSelectPanel = cardSelectPanel;
        
        var players = GameContext.Instance.Players;

        _cardSelectPanel.SubscribeToPlayers(players);

        GamePhaseManager = new GamePhaseManager(players);
    }
    internal void StartGame() {
        GamePhaseManager.StartHeroesPlacement();       
        OnGameStarted?.Invoke();
    }
    internal void HandlePlacementFinished() {
        //Player nextPlayer = _gamePhaseHeroesPlacement.GetNextPlayer();
        //if (nextPlayer != null) {
        //    Debug.Log("Next player color: " + nextPlayer.Color);
        //    _tokenPlacementViewModel.InitiatePlacing(nextPlayer, 1, 2);
        //} else {
        //    _gamePhaseHeroesPlacement = null;
        //    PlayerTurn(_gamePhaseHeroesPlacement.GetActivePlayer());
        //}
    }
    private void HadlePhaseComplete() {

    }
    private void PlayerTurn(Player player) {
        Debug.Log($"Game started.");                    
    }
}
