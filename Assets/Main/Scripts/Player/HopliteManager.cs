using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HopliteManager
{
    public const int TOTAL_HOPLITES = 15;    
    private List<HopliteUnit> _hoplites = new(); 
    public event System.Action OnChanged;

    public HopliteManager(Player player)
    {
        for (var id = 1; id <= TOTAL_HOPLITES; id++) 
        {
            _hoplites.Add(new HopliteUnit(player.Color, id));
        }
    }
    public bool TryTakeHoplite(out HopliteUnit hoplite)
    {
        
        foreach (var h in _hoplites)
        {
            if (!h.OnBoard)
            {
                h.OnBoard = true;
                hoplite = h;
                return true;
            }
        }
        hoplite = null;
        return false;
    }
    // public bool TryPlaceHoplite(RegionId regionId, out HopliteUnit hoplite)
    // {
    //     hoplite = null;
    //     foreach (var h in _hoplites) {
    //         if (h.OnBoard) continue;
    //         h.ChangeRegion(regionId);
    //         hoplite = h;
    //         OnChanged?.Invoke();
    //         return true;
    //     }
    //     return false;
    // }
}
