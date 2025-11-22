using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class GameManager
{
    private PlayerTurnManager _playerTurnManager;
    public GamePhaseManager GamePhaseManager;
    private LandTokenManager _landTokenManager = new();
    private CardSelectPanel _cardSelectPanel;
    private readonly TokenModelFactory _tokenModelFactory = new();

    public event Action OnGameStarted;

    public GameManager( GameData gameData, 
                        TokenPlacementManager placementManager, 
                        CardSelectPanel cardSelectPanel) {
        _playerTurnManager = new PlayerTurnManager(GameState.Instance.Players);
        _cardSelectPanel = cardSelectPanel;

        foreach (var player in GameState.Instance.Players) {
            _landTokenManager.Subscribe(player);
        }
        _cardSelectPanel.SubscribeToPlayers(_playerTurnManager.Players);
        GamePhaseManager = new GamePhaseManager(_playerTurnManager.Players, placementManager, _playerTurnManager);
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