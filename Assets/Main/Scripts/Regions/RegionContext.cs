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

    public event Action<PlayerColor> OnOwnerChanged;
    public event Action<TokenModel> TokenAdded;
    public event Action<TokenModel> OnTokenRemoved;

    [SerializeField] private List<TokenModel> _tokens = new();
    public IReadOnlyList<TokenModel> Tokens => _tokens;

    public RegionContext(RegionConfig regionCfg)
    {
        RegionId = RegionIdParser.Parse(regionCfg.RegionName);
        RegionConfig = regionCfg;
        OwnedBy = PlayerColor.Gray;
    }
    public void Place(TokenModel token)
    {
        token.RegionId = RegionId;
        _tokens.Add(token);         
        TokenAdded?.Invoke(token);
    }
    public void Take(TokenModel token)
    {
        token.RegionId = RegionId.Unknown;
        _tokens.Remove(token);        
        OnTokenRemoved?.Invoke(token);
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
        int counter = 0;
        foreach(var t in _tokens) {
            if (t  is HopliteModel hoplite) {
                if (hoplite.PlayerColor == color) {
                    counter++;
                }
            }
        }
        return counter;
    }
    public bool ContainsAnotherHero(PlayerColor color)
    {
        return _tokens.OfType<HeroModel>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHoplite(PlayerColor color)
    {
        return _tokens.OfType<HopliteModel>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHopliteOfColor(PlayerColor color, out HopliteModel hoplite)
    {
        foreach (var token in _tokens) {
            if (token is HopliteModel hs) {
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
        OnOwnerChanged?.Invoke(color);
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
}
