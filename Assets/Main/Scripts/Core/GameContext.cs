using System;
using System.Collections.Generic;
using UnityEngine;

public class GameContext
{
    private static GameContext _instance;
    public static GameContext Instance => _instance ??= new GameContext();
    private GameContext() { }

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
    public RegionsContext RegionDataRegistry { get; private set; }

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
        eventCards.AddRange(GameContent.Instance.MonsterCards);
        eventCards.AddRange(GameContent.Instance.QuestCards);
        _eventDeck = new CardDeck(eventCards);

        if (GameConfig.Instance.CardsEvent != null) {
            foreach (var card in GameConfig.Instance.CardsEvent) {
                if (card is CardQuest quest) {
                    if (_eventDeck.RemoveCard(quest)) {
                        Debug.Log("Removing cards!" + quest.Title);
                    }
                }
            }
        }

        _monsterAttackDeck = new CardDeck(GameContent.Instance.MonsterAttackCards);
        _combatCardsDeck = new CardDeck(CardLoader.Instance.CombatCards);
        _artifactDeck = new CardDeck(CardLoader.Instance.ArtifactCards);
        _blessingDeck = new CardDeck(CardLoader.Instance.BlessingCards);

        RegionDataRegistry = new(GameContent.Instance.RegionsConfig);

        foreach (var playerCfg in GameConfig.Instance.Players) {
            var player = new Player(playerCfg);
            _players.Add(player);
        }
    }
}
