using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class RegionRuntimeData
{
    public RegionId RegionId { get; private set; }
    public RegionStaticData RegionStaticData { get; private set; }
    public bool HasTemple { get; set; }
    public PlayerColor OwnedBy { get; set; }
    public List<Quest> ActiveQuests { get; private set; } = new();
    public bool IsFortified { get; private set; }

    [SerializeField] private List<Token> _tokens = new();
    public IReadOnlyList<Token> Tokens => _tokens;

    public event Action<PlayerColor> OnOwnerChanged;
    public event Action<RegionId, PlayerColor, int> OnHopliteCountChanged;
    public RegionRuntimeData(RegionStaticData regionData) {
        RegionId = RegionIdParser.Parse(regionData.RegionName);
        RegionStaticData = regionData;
        OwnedBy = PlayerColor.Gray;
    }
    public void RegisterToken<T>(T token) where T : Token {
        //Debug.Log("Registering token: " + token.TokenType.ToString());        
        
        if (token is HopliteStack hoplite) {
            if (ContainsAnotherHopliteOfColor(hoplite.PlayerColor, out var foundHoplite)) {
                hoplite.Count++;                
            } else {
                hoplite.Count = 1;
                _tokens.Add(hoplite);
            }
            SetHopliteCount(hoplite.PlayerColor, hoplite.Count);

            if (hoplite.Count >= RegionStaticData.PopulationStrength) {
                ChangeOwner(hoplite.PlayerColor);
            }
        } else {
            if (!_tokens.Contains(token)) {
                _tokens.Add(token);
            }
            if (token is Hero hero) {
                hero.RegionId = RegionId;
                hero.LandId = GetLandId(RegionStaticData.LandColor);
            }
        }
    }
    public void RemoveToken<T>(T token) where T : Token {
        //Debug.Log("Removig token: " + token.TokenType.ToString());
        if (token is HopliteStack hoplite) {
            if (hoplite.Count <= 1) {
                _tokens.Remove(token);
                ChangeOwner(PlayerColor.Gray);
            } else {
                hoplite.Count--;
                SetHopliteCount(hoplite.PlayerColor, hoplite.Count);
            }
        } else {
            _tokens.Remove(token);
            if (token is Hero hero) {
                hero.RegionId = RegionId.Unknown;
            }
        }
    }
    public bool FindToken(TokenType tokenType, PlayerColor color, out Token token) {
        foreach (Token t in _tokens) {
            if (t.Type == tokenType && (t is IPlayerOwned ownedToken) && ownedToken.PlayerColor == color) { 
                token = t;
                return true;
            }
        }
        token = null;
        return false;
    }
    public bool ContainsToken(TokenType tokenType) {
        return _tokens.Any(t => t.Type == tokenType);
    }
    public bool ContainsToken(PlayerColor color) {
        return _tokens.Any(t => t is IPlayerOwned owned && owned.PlayerColor == color);
    }
    public int GetHopliteCount(PlayerColor color) {
        var hoplite = _tokens.OfType<HopliteStack>().FirstOrDefault(h => h.PlayerColor == color);
        if (hoplite != null) {
            return hoplite.Count;
        }
        return 0;
    }
    public bool ContainsAnotherHero(PlayerColor color) {
        return _tokens.OfType<Hero>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHoplite(PlayerColor color) {
        return _tokens.OfType<HopliteStack>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHopliteOfColor(PlayerColor color, out HopliteStack hoplite) {
        //Debug.Log("Token count: " + _tokens.Count);
        foreach (var token in _tokens) {
            if (token is HopliteStack h) {
                //Debug.Log("Contains");
                if (h.PlayerColor == color) {
                    hoplite = h;
                    return true;
                }
            }
        }
        hoplite = null;
        return false;
    }
    private void SetHopliteCount(PlayerColor color, int count) {
        foreach (var token in _tokens.OfType<HopliteStack>()) {
            if (token.PlayerColor == color) {
                token.Count = count;
                OnHopliteCountChanged?.Invoke(RegionId, color, count);
            }
        }
    }
    private void ChangeOwner(PlayerColor color) {
        Debug.Log("Change owner!" +  color);
        OwnedBy = color;
        OnOwnerChanged?.Invoke(OwnedBy);
    }
    private LandId GetLandId(string landColor) {
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
}
