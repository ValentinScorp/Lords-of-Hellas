using System;
using System.Collections.Generic;
using UnityEngine;

internal class GameContext
{
    private static GameContext _instance;
    internal static GameContext Instance => _instance ??= new GameContext();
    private GameContext() { }

    private CardDeck _eventDeck;
    private CardDeck _monsterAttackDeck;
    private CardDeck _combatCardsDeck;
    private CardDeck _artifactDeck;
    private CardDeck _blessingDeck;
    private TemplePool _templePool = new();
    private Player _currentPlayer = null;
    private List<Player> _players = new List<Player>();
    internal CardDeck EventDeck => _eventDeck;
    internal CardDeck MonsterAttackDeck => _monsterAttackDeck;
    internal CardDeck CombatCardsDeck => _combatCardsDeck;
    internal CardDeck ArtifactDeck => _artifactDeck;
    internal CardDeck BlessingDeck => _blessingDeck;
    internal TemplePool TemplePool => _templePool;
    internal RegionsContext RegionRegistry { get; private set; }

    internal event Action<Player> OnPlayerChanged;
    internal Player CurrentPlayer {
        get => _currentPlayer;
        set {
            if (_currentPlayer == value) return;
            _currentPlayer = value;
            OnPlayerChanged?.Invoke(_currentPlayer);
        }
    }
    internal IReadOnlyList<Player> Players => _players;
    internal void Initialize() {
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

        RegionRegistry = new(GameContent.Instance.RegionsConfig);

        foreach (var playerCfg in GameConfig.Instance.Players) {
            var player = new Player(playerCfg);
            _players.Add(player);
        }
    }
}
