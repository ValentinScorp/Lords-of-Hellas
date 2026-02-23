using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HopliteManager
{
    public const int HopliatesMax = 15;
    private List<HopliteModel> _hoplites = new();
    private Action _HopliteChangedRegion;

    public HopliteManager(Player player)
    {
        _HopliteChangedRegion = player.HopliteRegionChanded;

        for (var id = 1; id <= HopliatesMax; id++) {
            var hoplite = new HopliteModel(player);
            hoplite.RegionChanged += HopliteChangedRegion;
            _hoplites.Add(hoplite);
        }
    }
    public bool TryTakeHoplite(out HopliteModel hoplite)
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
    internal int HoplitesOnBoard()
    {
        int count = 0;
        foreach(var h in _hoplites) {
            if (h.IsOnBoard())
                count++;
        }
        return count;
    }
    internal int HoplitesInHand()
    {
        return HopliatesMax - HoplitesOnBoard();
    }
    internal void ResetMove()
    {
        foreach(var hoplite in _hoplites) {
            hoplite.ResetMove();
        }
    }
    internal void HopliteChangedRegion(TokenModel token)
    {
        if (token.RegionId != RegionId.Unknown) {
            Debug.Log("Hoplite changed Region!");
            _HopliteChangedRegion.Invoke();
        }
    }
}
