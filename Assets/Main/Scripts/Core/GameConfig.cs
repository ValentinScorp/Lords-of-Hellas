using System;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    private static GameConfig _instance;
    public static GameConfig Instance => _instance ??= new GameConfig();
    private GameConfig() { }
    private List<CardData> _eventCards = new();
    private List<PlayerSetupConfig> _players = new();
    public List<PlayerSetupConfig> Players => _players;
    public List<CardData> CardsEvent => _eventCards; 
    public CardTemple TempleCard { get; set; }

    public void Initialize()
    {
        CreateDefaultPlayers();
        CreateDefaultTempleCard();
    }
    public void AddPlayer(PlayerSetupConfig player) {
        _players.Add(player);
    }
    public List<PlayerSetupConfig> CreateDefaultPlayers() { 
        if (_players.Count == 0) {
            PlayerSetupConfig player1 = new PlayerSetupConfig("Alice", Hero.Id.Achilles, PlayerColor.Red);
            PlayerSetupConfig player2 = new PlayerSetupConfig("Bob", Hero.Id.Heracles, PlayerColor.Blue);
            _players.Add(player1);
            _players.Add(player2);
        }
        return _players; 
    }
    public void CreateDefaultTempleCard()
    {
        if (GameContent.Instance.TryGetTempleCard(0, out var card)) {
            TempleCard = card;
        } else {
            Debug.LogWarning("Can't get default Temple Card!");
        }
    }
    public void AddEventCard(CardData card) {
        _eventCards.Add(card);
    }
}
