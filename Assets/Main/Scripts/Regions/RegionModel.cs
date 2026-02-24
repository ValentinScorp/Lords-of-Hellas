using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class RegionModel
{
    public RegionId RegionId { get; private set; }
    public RegionConfig RegionConfig { get; private set; }
    public bool HasTemple { get; set; }
    public PlayerColor OwnedBy { get; set; }
    public List<Quest> ActiveQuests { get; private set; } = new();
    public bool IsFortified { get; private set; }

    public event Action<PlayerColor> OwnerChanged;
    public event Action<TokenModel, int> TokenPlaced;
    public event Action<RegionModel> TemplePlaced;
    public event Action<TokenModel> TokenRemoved;

    [SerializeField] private List<TokenModel> _tokens = new();
    public IReadOnlyList<TokenModel> Tokens => _tokens;

    public RegionModel(RegionConfig regionCfg)
    {
        RegionId = RegionIdParser.Parse(regionCfg.RegionName);
        RegionConfig = regionCfg;
        OwnedBy = PlayerColor.Gray;
    }
    public void Place(TokenModel token, RegionNest nest = null)
    {
        if (token is HopliteModel hoplite) {
            PlaceHoplite(hoplite, nest);
        } else {
            if (nest is null)
                PlaceToken(token, -1);
            else
                PlaceToken(token, nest.Id);
        }
    }
    private void PlaceHoplite(HopliteModel hoplite, RegionNest nest)
    {
        if (TryFindHopliteStack(hoplite.PlayerColor, out var hopliteStack)) {
            hopliteStack.AddHoplite(hoplite);
            RecalcOwner();
        } else {
            var newHopliteStack = new HopliteStackModel(hoplite.PlayerColor);
            if (nest is null)
                nest = ServiceLocator.Get<RegionsView>().GetFreeNest(RegionId);
            newHopliteStack.SetBoardLocation(nest.RegionId, nest.Id);
            newHopliteStack.AddHoplite(hoplite);
            PlaceToken(newHopliteStack, nest.Id);
            RecalcOwner();            
        }
    }
    private void PlaceToken(TokenModel token, int nestId)
    {
        token.SetBoardLocation(RegionId, nestId);
        _tokens.Add(token);
        TokenPlaced?.Invoke(token, nestId);
    }
    private void RecalcOwner()
    {
        HopliteStackModel hopliteStack = null;
        int hopliteStackCounter = 0;
        foreach (TokenModel t in _tokens) {
            if (t is HopliteStackModel hopliteStackModel) {
                hopliteStack = hopliteStackModel;
                hopliteStackCounter++;
            }
        }
        if (hopliteStackCounter == 1) {
            if (hopliteStack.Count >= RegionConfig.PopulationStrength) {
                ChangeOwner(hopliteStack.PlayerColor);
            }
        }
    }   
    public void Take(TokenModel token)
    {
        if (token is HopliteModel hoplite) {
            TakeHoplite(hoplite);
        } else {
            token.ClearNest();
            _tokens.Remove(token);
            TokenRemoved?.Invoke(token);
        }
    }
     private void TakeHoplite(HopliteModel hoplite)
    {
        if (TryFindHopliteStack(hoplite.PlayerColor, out var hopliteStack)) {
            hopliteStack.RemoveHoplite(hoplite);
            if (hopliteStack.Count == 0) {
                Take(hopliteStack); 
            }
        }
    }
    public bool TryFindToken(TokenType tokenType, PlayerColor color, out TokenModel token)
    {
        foreach (TokenModel t in _tokens) {
            if (t.Type == tokenType && (t.PlayerColor == color)) {
                token = t;
                return true;
            }
        }
        token = null;
        return false;
    }
    public bool ContainsToken(TokenType tokenType)
    {
        return _tokens.Any(t => t.Type == tokenType);
    }
    public bool ContainsToken(PlayerColor color)
    {
        return _tokens.Any(t => t.PlayerColor == color);
    }
    public int GetHopliteCount(PlayerColor color)
    {
        foreach(var t in _tokens) {
            if (t  is HopliteStackModel hopliteStack) {
                if (hopliteStack.PlayerColor == color) {
                    return hopliteStack.Count;
                }
            }
        }
        return 0;
    }
    public bool ContainsHeroAnotherColor(PlayerColor color)
    {
        return _tokens.OfType<HeroModel>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsHeroSameColor(PlayerColor color)
    {
        return _tokens.OfType<HeroModel>().Any(h => h.PlayerColor == color);
    }
    public bool ContainsHopliteAnotherColor(PlayerColor color)
    {
        if (_tokens.OfType<HopliteStackModel>().Any(h => h.PlayerColor != color)) {
            return true;
        }
        return false;
    }
    public bool ContainsHopliteSameColor(PlayerColor color, out HopliteStackModel hoplite)
    {
        foreach (var token in _tokens) {
            if (token is HopliteStackModel hs) {
                if (hs.PlayerColor == color) {
                    hoplite = hs;
                    return true;
                }
            }
        }
        hoplite = null;
        return false;
    }

    private void ChangeOwner(PlayerColor color)
    {
        OwnedBy = color;
        OwnerChanged?.Invoke(color);
    }
    private LandId GetLandId(string landColor)
    {
        switch (landColor.ToLowerInvariant()) {
            case "red": return LandId.Red;
            case "yellow": return LandId.Yellow;
            case "blue": return LandId.Blue;
            case "green": return LandId.Green;
            case "brown": return LandId.Brown;
            default:
                Debug.LogError($"Unknown land color: {landColor}");
                return LandId.Red;
        }
    }
    public bool TryGetToken(TokenType tokenType, PlayerColor color, out TokenModel token)
    {
        foreach (var t in _tokens) {
            if (t.Type == tokenType && t.PlayerColor == color) {
                token = t;
                return true;
            }
        }
        token = null;
        return false;
    }
    private bool TryFindHopliteStack(PlayerColor playerColor, out HopliteStackModel hopliteStack)
    {
        if (TryGetToken(TokenType.HopliteStack, playerColor, out var token)) {
            if (token is HopliteStackModel hs) {
                hopliteStack = hs;
                return true;
            }
        }
        hopliteStack = null;
        return false;
    }

    internal bool TryPlaceTemple()
    {
        if (!RegionConfig.HasShrine) return false;
        if (HasTemple) return false;

        HasTemple = true;
        TemplePlaced?.Invoke(this);
        return true;
    }
}
