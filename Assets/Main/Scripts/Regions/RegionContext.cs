using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public void RegisterHopliteUnit(HopliteModel hopliteUnit)
    {
        if (ContainsAnotherHopliteOfColor(hopliteUnit.PlayerColor, out var foundHopliteStack)) {
            foundHopliteStack.AddHoplite(hopliteUnit);
        } else {
            var hopliteStack = new HopliteStackModel(hopliteUnit);
            AddToken(hopliteStack);
        }        
    }
    public bool UnregisterHopliteUnit(HopliteModel hopliteUnit)
    {
        if (!ContainsAnotherHopliteOfColor(hopliteUnit.PlayerColor, out var hopliteStack)) {
            Debug.Log($"Can't unregister hopliteUnit from region {RegionId}");
            return false;
        }

        if (hopliteStack.RemoveHoplite(hopliteUnit)) {
            if (hopliteStack.Count == 0) {
                RemoveToken(hopliteStack);
            }
            return true;
        }
        return false;        
    }
    public bool TryRegisterToken(TokenModel token)
    {
        AddToken(token);
        return true;
    }
    public void RegisterToken(TokenModel token)
    {
        if (token is HopliteStackModel hopliteStack) {
            int hopliteCount = 0;
            if (ContainsAnotherHopliteOfColor(hopliteStack.PlayerColor, out var foundHopliteStack)) {
                var hoplite = hopliteStack.RemoveHoplite();
                hoplite.ChangeRegion(RegionId);
                foundHopliteStack.AddHoplite(hoplite);
                hopliteCount = foundHopliteStack.Count;
            } else {
                hopliteCount = hopliteStack.Count;
                hopliteStack.ChangeHoplitesRegion(RegionId);
                AddToken(hopliteStack);                
            }
            if (hopliteCount >= RegionConfig.PopulationStrength) {
                ChangeOwner(hopliteStack.PlayerColor);
            }
        } else if (token is HeroModel hero) {
            hero.RegionId = RegionId;
            AddToken(hero);
        }
    }
    public void RemoveToken<T>(T token) where T : TokenModel
    {
        if (token is HopliteStackModel hopliteStack) {
            if (hopliteStack.Count <= 1) {
                RemoveToken(token);
            } else {
                hopliteStack.RemoveHoplite();
            }
        } else {
            RemoveToken(token);
        }
    }
    public bool FindToken(TokenType tokenType, PlayerColor color, out TokenModel token)
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
        var hoplite = _tokens.OfType<HopliteStackModel>().FirstOrDefault(h => h.PlayerColor == color);
        if (hoplite != null) {
            return hoplite.Count;
        }
        return 0;
    }
    public bool ContainsAnotherHero(PlayerColor color)
    {
        return _tokens.OfType<HeroModel>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHoplite(PlayerColor color)
    {
        return _tokens.OfType<HopliteStackModel>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHopliteOfColor(PlayerColor color, out HopliteStackModel hopliteStack)
    {
        foreach (var token in _tokens) {
            if (token is HopliteStackModel hs) {
                if (hs.PlayerColor == color) {
                    hopliteStack = hs;
                    return true;
                }
            }
        }
        hopliteStack = null;
        return false;
    }

    private void ChangeOwner(PlayerColor color)
    {
        OwnedBy = color;
        // EventBus.SendEvent(new RegionOwnerEvent(RegionId, color));
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
    private void AddToken(TokenModel token)
    {
        token.RegionId = RegionId;
        _tokens.Add(token);         
        TokenAdded?.Invoke(token);
    }
    private void RemoveToken(TokenModel token)
    {
        token.RegionId = RegionId.Unknown;
        _tokens.Remove(token);        
        OnTokenRemoved?.Invoke(token);
    }
}
