using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Board;

public class GameState
{
    private static GameState _instance;
    public static GameState Instance => _instance ??= new GameState();
    private GameState() { }

    private CardDeck _eventDeck;
    private CardDeck _monsterAttackDeck;
    private CardDeck _combatCardsDeck;
    private CardDeck _artifactDeck;
    private CardDeck _blessingDeck;
    private TemplePool _templePool = new();
    private Player _currentPlayer = null;
    private List<Player> _players = new List<Player>();
    public CardDeck EventDeck => _eventDeck;
    public CardDeck MonsterAttackDeck => _monsterAttackDeck;
    public CardDeck CombatCardsDeck => _combatCardsDeck;
    public CardDeck ArtifactDeck => _artifactDeck;
    public CardDeck BlessingDeck => _blessingDeck;
    public TemplePool TemplePool => _templePool;

    private List<RegionStatus> _regionStatuses = new();
    public List<RegionStatus> RegionStatuses => _regionStatuses;

    public event Action<Player> OnPlayerChanged;
    public Player CurrentPlayer {
        get => _currentPlayer;
        set {
            if (_currentPlayer == value) return;
            _currentPlayer = value;
            OnPlayerChanged?.Invoke(_currentPlayer);
        }
    }
    public IReadOnlyList<Player> Players => _players;
    public void Initialize() {

        List<CardData> eventCards = new List<CardData>();
        eventCards.AddRange(GameData.Instance.MonsterCards);
        eventCards.AddRange(GameData.Instance.QuestCards);
        _eventDeck = new CardDeck(eventCards);
        var selectedCards = GameConfig.Instance.CardsEvent;

        if (selectedCards != null) {
            foreach (var card in selectedCards) {
                if (card is CardQuest quest) {
                    if (_eventDeck.RemoveCard(quest)) {
                        Debug.Log("Removing cards!" + quest.Title);
                    }
                }
            }
        }

        _monsterAttackDeck = new CardDeck(GameData.Instance.MonsterAttackCards);
        _combatCardsDeck = new CardDeck(CardLoader.Instance.CombatCards);
        _artifactDeck = new CardDeck(CardLoader.Instance.ArtifactCards);
        _blessingDeck = new CardDeck(CardLoader.Instance.BlessingCards);

        foreach (var pConfig in GameData.Instance.PlayerConfigs) {
            var player = new Player(pConfig);
            _players.Add(player);
        }
    }  
}
