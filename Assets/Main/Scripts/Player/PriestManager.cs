using System;
using System.Collections.Generic;
using UnityEngine;

internal class PriestManager
{
    internal const int MaxPriests = 4;
    private readonly List<PriestModel> _priests = new();
    internal List<PriestModel> Priests => _priests;
    internal event Action<PriestManager, PriestModel> PriestChangedStatus;
    internal PriestManager(PlayerColor color)
    {
        for (int i = 0; i < MaxPriests; i++) {
            var priest = new PriestModel(color);
            priest.PriestChangedStatus += OnPriestChangesStatus;
            _priests.Add(new PriestModel(color));
        }
    }

    internal int InPool {
        get {
            int count = 0;
            foreach (var priest in _priests) {
                if (priest.PlacedAt == PriestModel.Placement.InPool) {
                    count++;
                }
            }
            return count;
        }
    }

    internal bool TryMoveToPool()
    {
        foreach (var priest in _priests) {
            if (priest.TryMoveToPool()) {
                return true;
            }
        }

        return false;
    }
    private void OnPriestChangesStatus(PriestModel priest)
    {
        PriestChangedStatus?.Invoke(this, priest);
    }

    internal void RefreshStatus()
    {
        PriestChangedStatus?.Invoke(this, null);
    }

}
