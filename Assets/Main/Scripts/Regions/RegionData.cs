using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class RegionData
{
    public RegionId RegionId { get; private set; }
    public RegionConfig RegionStaticData { get; private set; }
    public bool HasTemple { get; set; }
    public PlayerColor OwnedBy { get; set; }
    public List<Quest> ActiveQuests { get; private set; } = new();
    public bool IsFortified { get; private set; }

    public event Action<PlayerColor> OnOwnerChanged;

    [SerializeField] private List<TokenModel> _tokens = new();
    public IReadOnlyList<TokenModel> Tokens => _tokens;

    public RegionData(RegionConfig regionCfg)
    {
        RegionId = RegionIdParser.Parse(regionCfg.RegionName);
        RegionStaticData = regionCfg;
        OwnedBy = PlayerColor.Gray;
    }
    public void RegisterEntity<T>(T token) where T : TokenModel
    {
        if (token is HopliteStack hopliteStack) {
            int hopliteCount = 0;
            if (ContainsAnotherHopliteOfColor(hopliteStack.PlayerColor, out var foundHopliteStack)) {
                var hoplite = hopliteStack.RemoveHoplite();
                hoplite.ChangeRegion(RegionId);
                foundHopliteStack.AddHoplite(hoplite);
                hopliteCount = foundHopliteStack.Count;
            } else {
                hopliteCount = hopliteStack.Count;
                hopliteStack.ChangeHoplitesRegion(RegionId);
                _tokens.Add(hopliteStack);
            }
            if (hopliteCount >= RegionStaticData.PopulationStrength) {
                ChangeOwner(hopliteStack.PlayerColor);
            }
        } else if (token is Hero hero) {
            hero.RegionId = RegionId;
            hero.LandId = GetLandId(RegionStaticData.LandColor);
            _tokens.Add(hero);
        }
    }
    public void RemoveToken<T>(T token) where T : TokenModel
    {
        if (token is HopliteStack hoplite) {
            if (hoplite.Count <= 1) {
                _tokens.Remove(token);
                ChangeOwner(PlayerColor.Gray);
            } else {
                hoplite.RemoveHoplite();
            }
        } else {
            _tokens.Remove(token);
            if (token is Hero hero) {
                hero.RegionId = RegionId.Unknown;
            }
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
        var hoplite = _tokens.OfType<HopliteStack>().FirstOrDefault(h => h.PlayerColor == color);
        if (hoplite != null) {
            return hoplite.Count;
        }
        return 0;
    }
    public bool ContainsAnotherHero(PlayerColor color)
    {
        return _tokens.OfType<Hero>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHoplite(PlayerColor color)
    {
        return _tokens.OfType<HopliteStack>().Any(h => h.PlayerColor != color);
    }
    public bool ContainsAnotherHopliteOfColor(PlayerColor color, out HopliteStack hopliteStack)
    {
        foreach (var token in _tokens) {
            if (token is HopliteStack hs) {
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
}
