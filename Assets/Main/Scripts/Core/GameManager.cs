using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class GameManager
{
    private PlayerTurnController _playerTurn;
    public GamePhaseManager GamePhaseManager;
    private LandTokenManager _landTokenManager = new();
    private CardSelectPanel _cardSelectPanel;
    public event Action OnGameStarted;

    public GameManager(CardSelectPanel cardSelectPanel) {
        _cardSelectPanel = cardSelectPanel;

        foreach (var player in GameContext.Instance.Players) {
            _landTokenManager.Subscribe(player);
        }
        var players = GameContext.Instance.Players;

        _cardSelectPanel.SubscribeToPlayers(players);

        GamePhaseManager = new GamePhaseManager(players);
    }
    public void StartGame() {
        GamePhaseManager.StartHeroesPlacement();       
        OnGameStarted?.Invoke();
    }
    public void HandlePlacementFinished() {
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