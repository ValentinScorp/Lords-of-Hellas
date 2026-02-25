using System.Collections.Generic;
using UnityEngine;

public class PriestManager
{
    private const int _MaxPriests = 4;
    private List<PriestModel> _priests = new();

    public PriestManager(PlayerColor color)
    {
        for (int i = 0; i < _MaxPriests; i++) {
            _priests.Add(new PriestModel(color));
        }
    }

    public int InPool {
        get {
            int count = 0;
            foreach (var priest in _priests) {
                if (priest.PlacedAt == PriestModel.Placement.Pool) {
                    count++;
                }
            }
            return count;
        }
    }

    public bool MoveToPool()
    {
        foreach (var priest in _priests) {
            if (priest.TryMoveToPool()) {
                return true;
            }
        }

        return false;
    }
}
