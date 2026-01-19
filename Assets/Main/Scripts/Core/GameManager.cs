using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class GameManager
{
    private TurnManager _playerTurnManager;
    public GamePhaseManager GamePhaseManager;
    public PlayerDataManager PlayerDataManager;
    private LandTokenManager _landTokenManager = new();
    private CardSelectPanel _cardSelectPanel;
    public event Action OnGameStarted;

    public GameManager( GameContent gameData, 
                        TokenPlacementManager placementManager, 
                        CardSelectPanel cardSelectPanel) {
        _playerTurnManager = new TurnManager(GameContext.Instance.Players);
        _cardSelectPanel = cardSelectPanel;

        foreach (var player in GameContext.Instance.Players) {
            _landTokenManager.Subscribe(player);
        }
        _cardSelectPanel.SubscribeToPlayers(_playerTurnManager.Players);
        GamePhaseManager = new GamePhaseManager(_playerTurnManager.Players, placementManager, _playerTurnManager);
        PlayerDataManager = new PlayerDataManager(_playerTurnManager.Players);
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