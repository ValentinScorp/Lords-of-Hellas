using System;
using System.Collections.Generic;

[Serializable]
internal class HopliteManager
{
    internal const int MaxHoplites = 15;
    private List<HopliteModel> _hoplites = new();
    internal Action<HopliteManager, TokenModel> HopliteChangedRegion;    

    internal HopliteManager(PlayerColor color)
    {
        for (var id = 1; id <= MaxHoplites; id++) {
            var hoplite = new HopliteModel(color);
            hoplite.RegionChanged += OnHopliteChangedRegion;
            _hoplites.Add(hoplite);
        }
    }
    internal bool TryTakeHoplite(out HopliteModel hoplite)
    {
        foreach (var h in _hoplites) {
            if (!h.OnBoard) {
                h.OnBoard = true;
                hoplite = h;
                return true;
            }
        }
        hoplite = null;
        return false;
    }
    internal int HoplitesOffBoard()
    {
        return MaxHoplites - HoplitesOnBoard();
    }
    internal int HoplitesOnBoard()
    {
        int count = 0;
        foreach(var h in _hoplites) {
            if (h.IsOnBoard())
                count++;
        }
        return count;
    }
    internal void ResetMove()
    {
        foreach(var hoplite in _hoplites) {
            hoplite.ResetMove();
        }
    }
    internal void RefreshStatus()
    {
        HopliteChangedRegion?.Invoke(this, null);
    }
    internal void OnHopliteChangedRegion(TokenModel token)
    {
        if (token.RegionId != RegionId.Unknown) {
            HopliteChangedRegion?.Invoke(this, token);
        }
    }
}
