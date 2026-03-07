using System;
using System.Collections.Generic;
using UnityEngine;

internal class GameConfig
{
    private static GameConfig _instance;
    internal static GameConfig Instance => _instance ??= new GameConfig();
    private GameConfig() { }
    private List<CardData> _eventCards = new();
    private List<PlayerSetupConfig> _players = new();
    internal List<PlayerSetupConfig> Players => _players;
    internal List<CardData> CardsEvent => _eventCards; 
    internal CardTemple TempleCard { get; set; }

    internal void Initialize()
    {
        CreateDefaultPlayers();
        CreateDefaultTempleCard();
    }
    internal void AddPlayer(PlayerSetupConfig player) {
        _players.Add(player);
    }
    internal List<PlayerSetupConfig> CreateDefaultPlayers() { 
        if (_players.Count == 0) {
            PlayerSetupConfig player1 = new PlayerSetupConfig("Alice", HeroModel.Id.Achilles, PlayerColor.Red);
            PlayerSetupConfig player2 = new PlayerSetupConfig("Bob", HeroModel.Id.Perseus, PlayerColor.Blue);
            _players.Add(player1);
            _players.Add(player2);
        }
        return _players; 
    }
    internal void CreateDefaultTempleCard()
    {
        if (GameContent.Instance.TryGetTempleCard(0, out var card)) {
            TempleCard = card;
        } else {
            Debug.LogWarning("Can't get default Temple Card!");
        }
    }
    internal void AddEventCard(CardData card) {
        _eventCards.Add(card);
    }
}
