using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HopliteManager
{
    public const int TOTAL_HOPLITES = 15;
    private List<HopliteModel> _hoplites = new();

    public HopliteManager(Player player)
    {
        for (var id = 1; id <= TOTAL_HOPLITES; id++) {
            _hoplites.Add(new HopliteModel(player.Color));
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
}
