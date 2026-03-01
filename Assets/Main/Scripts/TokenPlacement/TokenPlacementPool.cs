using System.Collections.Generic;
using UnityEngine;

internal class TokenPlacementPool
{
    private List<HeroModel> _hero = new List<HeroModel>();
    private List<HopliteStackModel> _hoplites = new List<HopliteStackModel>();
    internal void SetPlacementTargets(Player player) {
        Reset();

        _hero.Add(player.Hero);
        var hs1 = new HopliteStackModel(player.Color);
        var hs2 = new HopliteStackModel(player.Color);
        hs1.AddHoplite(player.TakeHoplite());
        hs2.AddHoplite(player.TakeHoplite());
        _hoplites.Add(hs1);
        _hoplites.Add(hs2);
    }
    internal void Reset() {
        _hero.Clear();
        _hoplites.Clear();
    }
    internal bool CanPlace(TokenType type) {
        return type switch {
            TokenType.HopliteStack => _hoplites.Count > 0,
            TokenType.Hero => _hero.Count > 0,
            _ => false
        };
    }
    internal TokenModel TakeToken(TokenType? type)
    {
        switch (type)
        {
            case TokenType.HopliteStack:
                if (_hoplites.Count == 0) return null;
                var hopliteStack = _hoplites[^1];
                _hoplites.RemoveAt(_hoplites.Count - 1);
                return hopliteStack;
            case TokenType.Hero:
                if (_hero.Count == 0) return null;
                var hero = _hero[^1];
                _hero.RemoveAt(_hero.Count - 1);
                return hero;
            default:
                Debug.LogWarning($"[TokenPlacementTracker] Unknown token type: {type}");
                return null;
        }
    }
    internal bool AllPlaced()
    {
        return (_hero.Count == 0 && _hoplites.Count == 0);
    }
}
