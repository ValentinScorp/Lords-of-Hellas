using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
internal class Player
{
    private List<LandId> _landTokens = new();
    internal List<LandId> LandTokens => _landTokens;
    internal List<CardArtifact> _artifactCards = new();
    internal readonly HopliteManager _hopliteManager;
    internal PriestManager _priestManager;
    internal string _name;
    internal List<CardCombat> _combatCards = new();
    internal HopliteManager HopliteManager => _hopliteManager;
    internal PriestManager PriestManager =>_priestManager;
    internal List<CardArtifact> ArtifactCards => _artifactCards;
    internal List<CardCombat> CombatCards => _combatCards;
    internal HeroModel Hero { get; set; }
    internal PlayerColor Color { get; set; }
    internal int PriestsInPool => _priestManager.InPool;
    internal int HoplitesOnBoard => _hopliteManager.HoplitesOnBoard();
    internal int HoplitesInHand => _hopliteManager.HoplitesOffBoard();
    internal event Action<Player, int, Action> OnArtifactCardSelect;

    internal Player(PlayerSetupConfig playerConfig)
    {
        _name = playerConfig.PlayerName;
        Color = playerConfig.PlayerColor;
        Hero = new HeroModel(playerConfig.HeroId, this);
        _hopliteManager = new(Color);
        _priestManager = new(Color);
    }

    internal void SelectOneOfArtifactCards(int cardCount, Action onCompleted)
    {
        OnArtifactCardSelect?.Invoke(this, cardCount, onCompleted);
    }
    internal void TakeArtifactCard(CardArtifact card)
    {
        _artifactCards.Add(card);
        // OnPlayerInfoChange?.Invoke(this);
    }
    internal void ApplyHeroStartingBonus(Action onCompleted)
    {
        if (Hero is not null) {
            Hero.ApplyStartinBonus(this, onCompleted);
        } else {
            Debug.Log("Can`t apply starting bonus!");
            onCompleted?.Invoke();
        }
    }
    internal void TakeCombatCards(int count)
    {
        List<CardData> drawnCards = GameContext.Instance.CombatCardsDeck.DrawMultiple(count);
        _combatCards.AddRange(drawnCards.Cast<CardCombat>());
        // OnPlayerInfoChange?.Invoke(this);
    }
    internal bool TryTakeHoplite(out HopliteModel hoplite)
    {
        // Debug.Log("Taking hoplite from player!");
        var h = TakeHoplite();
        if (h is not null) {
            hoplite = h;
            // OnPlayerInfoChange?.Invoke(this);
            return true;
        }
        hoplite = null;
        return false;
    }
    internal HopliteModel TakeHoplite()
    {
        if (_hopliteManager.TryTakeHoplite(out var hoplite) && hoplite != null) {
            hoplite.SetOwner(Color);
            return hoplite;
        }
        return null;
    }
    internal void ResetHoplitesMove()
    {
        _hopliteManager.ResetMove();
    }
    internal void HopliteRegionChanded(HopliteManager manager, TokenModel token)
    {
        // OnPlayerInfoChange?.Invoke(this);
    }

    internal void PlacePriestInPool()
    {
        if (!_priestManager.TryMoveToPool()) {
            Debug.LogWarning($"Player {Color} has no available priest to place in pool");
            return;
        }

        // OnPlayerInfoChange?.Invoke(this);
    }

    internal bool ReceiveToken(LandId landId)
    {
        if (landId == LandId.Unknown) {
            Debug.LogWarning($"Player {Color} cannot receive Unknown land token.");
            return false;
        }

        if (_landTokens.Contains(landId)) {
            Debug.LogWarning($"Player {Color} already has token {landId}.");
            return false;
        }

        _landTokens.Add(landId);
        return true;
    }
    internal bool RemoveToken(LandId landId)
    {
        if (landId == LandId.Unknown) {
            Debug.LogWarning($"Player {Color} cannot remove Unknown land token.");
            return false;
        }

        if (!_landTokens.Remove(landId)) {
            Debug.LogWarning($"Player {Color} does not have token {landId}.");
            return false;
        }

        return true;
    }
}
