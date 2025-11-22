using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player
{
    [SerializeField] private PlayerColor _color;
    [SerializeField] private List<RegionId> _controlledRegions;
    [SerializeField] private List<CardArtifact> _artifactCards = new();
    private HopliteManager _hopliteManager = new();
    private PriestManager _priestManager = new();

    private string _name;
    private List<CardCombat> _combatCards = new();

    public List<CardArtifact> ArtifactCards => _artifactCards;
    public List<CardCombat> CombatCards => _combatCards;
    public Hero Hero { get; set; }
    public PlayerColor Color { get; set; }
    public int PriestsInPool => _priestManager.InPool;
    public int HoplitesOnBoard => _hopliteManager.OnBoard;

    public event Action<Player, int, Action> OnArtifactCardSelect;
    public event Action<Player, LandId> OnAddLandToken;
    public event Action<Player> OnPlayerInfoChange;

    public Player(PlayerSetupConfig playerConfig) {
        _name = playerConfig.PlayerName;
        Color = playerConfig.PlayerColor;
        Hero = new Hero(playerConfig.HeroId, this);
    }
    public void AddLandToken() {
        OnAddLandToken?.Invoke(this, Hero.LandId);
    }
    public void SelectOneOfArtifactCards(int cardCount, Action onCompleted) {
        OnArtifactCardSelect?.Invoke(this, cardCount, onCompleted);
    }
    public void TakeArtifactCard(CardArtifact card) {
        _artifactCards.Add(card);
        OnPlayerInfoChange?.Invoke(this);
    }
    public void ApplyHeroStartingBonus(Action onCompleted) {
        if (Hero != null) {
            Hero.ApplyStartinBonus(this, onCompleted);
        } else {
            Debug.Log("Can`t apply starting bonus!");
            onCompleted?.Invoke();
        }
    }
    public void TakeCombatCards(int count) {
        List<CardData> drawnCards = GameState.Instance.CombatCardsDeck.DrawMultiple(count);
        _combatCards.AddRange(drawnCards.Cast<CardCombat>());
        OnPlayerInfoChange?.Invoke(this);
    }
}
