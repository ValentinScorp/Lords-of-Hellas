using System.Collections.Generic;
using UnityEngine;

public class HopliteManager
{
    public const int TOTAL_HOPLITES = 15;
    public int OnBoard { get; private set; }
    public int InHand => TOTAL_HOPLITES - OnBoard;
    private List<Hoplite> _hoplites = new(); 
    public event System.Action OnChanged;

    public HopliteManager()
    {
        for (var id = 1; id <= TOTAL_HOPLITES; id++) 
        {
            _hoplites.Add(new Hoplite(id));
        }
    }
    public bool TryPlaceHoplite(RegionId regionId, out Hoplite hoplite)
    {
        hoplite = null;
        foreach (var h in _hoplites) {
            if (h.OnBoard) continue;
            h.PlaceOnBoard(regionId);
            hoplite = h;
            OnBoard++;
            OnChanged?.Invoke();
            return true;
        }
        return false;
    }
    public bool TryPlaceOnBoard(int count) {
        if (CanPlaceOnBoard(count)) {
            OnBoard += count;
            OnChanged?.Invoke();
            return true;
        }
        return false;
    }
    public bool CanPlaceOnBoard(int count) {
        if (count == 0) {
            Debug.LogWarning("Can't Place on board 0 Hoplites. Logical error!");
            return false;
        }
        if (count <= InHand) return true;
        return false;
    }
    public bool TryReturnToHand(int count) {
        if (CanReturnToHand(count)) {
            OnBoard -= count;
            OnChanged?.Invoke();
            return true;
        }
        return false;
    }
    public bool CanReturnToHand(int count) {
        if (count == 0) {
            Debug.LogWarning("Can't Return to hand 0 Hoplites. Logical error!");
            return false;
        }
        if (count <= OnBoard) return true;
        return false;
    }
}
