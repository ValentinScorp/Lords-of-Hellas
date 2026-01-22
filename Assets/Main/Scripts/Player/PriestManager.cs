using UnityEngine;

public class PriestManager
{
    public const int TOTAL_PRIESTS = 4;
    public int OnMonuments { get; private set; }
    public int InPool { get; private set; }
    public int InHand => TOTAL_PRIESTS - OnMonuments - InPool;

    public bool MoveToPool() {
        if (InHand <= 0) return false;
        InPool++;
        OnChanged?.Invoke();
        return true;
    }
    public bool PlaceOnMonument() {
        if (InPool <= 0) return false;
        InPool--;
        OnMonuments++;
        OnChanged?.Invoke();
        return true;
    }
    public bool ReturnToHand() {
        if (OnMonuments <= 0) return false;
        OnMonuments--;
        OnChanged?.Invoke();
        return true;
    }    
    public bool TrySacrifice(int count) {
        if (!CanSacrifice(count)) return false;
        InPool -= count;
        return true;
    }
    public bool CanSacrifice(int count) {
        if (count == 0) {
            Debug.LogWarning("Can't Sacrifice 0 Priests. Logical error!");
            return false;
        }
        return InPool >= count;
    }
    public event System.Action OnChanged;

}