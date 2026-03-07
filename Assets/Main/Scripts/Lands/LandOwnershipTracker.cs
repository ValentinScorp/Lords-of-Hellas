using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class LandOwnershipTracker
{
    private readonly Dictionary<LandId, List<RegionModel>> _regions = new();

    internal event Action<LandId, PlayerColor> LandOwnerChanged;

    internal void Subscribe(RegionsContext regions)
    {
        if (regions is null) return;
        
        regions.OwnerChanged += OnOwnerChanged;

        BuildRegionsIndex(regions);
    }
    private void BuildRegionsIndex(RegionsContext context)
    {
        _regions.Clear();
        if (context is null || context.RegionsContextList is null) return;

        foreach (var region in context.RegionsContextList) {
            if (region is null) continue;

            if (region.LandId == LandId.Unknown) {
                Debug.LogError($"[LandOwnershipWatcher] Region {region.RegionId} has LandId.Unknown");
                continue;
            }

            if (!_regions.TryGetValue(region.LandId, out var list)) {
                list = new List<RegionModel>();
                _regions[region.LandId] = list;
            }

            list.Add(region);
        }
    }

    private void OnOwnerChanged(RegionsContext context, RegionModel region)
    {
        if (region is null) return;
        if (region.LandId == LandId.Unknown) return;

        if (!_regions.TryGetValue(region.LandId, out var regionsOfLand) || regionsOfLand.Count == 0) return;

        if (region.OwnedBy != PlayerColor.Grey) {
            if(regionsOfLand.All(r => r.OwnedBy == region.OwnedBy)) {
                LandOwnerChanged?.Invoke(region.LandId, region.OwnedBy);
            }
        }
    }    
}
