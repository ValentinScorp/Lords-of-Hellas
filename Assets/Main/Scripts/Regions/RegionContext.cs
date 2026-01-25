using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RegionContext
{
    public RegionId RegionId { get; private set; }
    public RegionConfig RegionConfig { get; private set; }
    public bool HasTemple { get; set; }
    public PlayerColor OwnedBy { get; set; }
    public List<Quest> ActiveQuests { get; private set; } = new();
    public bool IsFortified { get; private set; }

    public event Action<PlayerColor> OwnerChanged;
    public event Action<TokenModel, TokenNest> TokenPlaced;
    public event Action<TokenModel> TokenRemoved;

    [SerializeField] private List<TokenModel> _tokens = new();
    public IReadOnlyList<TokenModel> Tokens => _tokens;

    public RegionContext(RegionConfig regionCfg)
    {
        RegionId = RegionIdParser.Parse(regionCfg.RegionName);
        RegionConfig = regionCfg;
        OwnedBy = PlayerColor.Gray;
    }
    public void Place(TokenModel token, TokenNest nest = null)
    {
        if (token is HopliteModel hoplite) {
            if (TryFindHopliteStack(hoplite.PlayerColor, out var hopliteStack)) {
                hopliteStack.AddHoplite(hoplite);
                return;
            } else {
                var newHopliteStack = new HopliteStackModel(hoplite.PlayerColor);
                newHopliteStack.AddHoplite(hoplite);
                token = newHopliteStack;                
            }
        }   
        token.RegionId = RegionId;
        _tokens.Add(token);         
        TokenPlaced?.Invoke(token, nest);
    }
    public void Take(TokenModel token)
    {
        token.RegionId = RegionId.Unknown;
        _tokens.Remove(token);        
        TokenRemoved?.Invoke(token);
    }
    public bool TryFindToken(TokenType tokenType, PlayerColor color, out TokenModel token)
    {
        foreach (TokenModel t in _tokens) {
            if (t.Type == tokenType && (t is IPlayerOwned ownedToken) && ownedToken.PlayerColor == color) {
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
        return _tokens.Any(t => t is IPlayerOwned owned && owned.PlayerColor == color);
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
            Debug.Log("Finding HopliteStack succeed!");
            return true;
        }
        Debug.Log("Finding HopliteStack fail!");
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
            if (t.Type == tokenType && t is IPlayerOwned owned && owned.PlayerColor == color) {
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
}
