using System.Collections.Generic;
using UnityEngine;

internal class PriestManager
{
    private const int _MaxPriests = 4;
    private List<PriestModel> _priests = new();

    internal PriestManager(PlayerColor color)
    {
        for (int i = 0; i < _MaxPriests; i++) {
            _priests.Add(new PriestModel(color));
        }
    }

    internal int InPool {
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

    internal bool MoveToPool()
    {
        foreach (var priest in _priests) {
            if (priest.TryMoveToPool()) {
                return true;
            }
        }

        return false;
    }
}
