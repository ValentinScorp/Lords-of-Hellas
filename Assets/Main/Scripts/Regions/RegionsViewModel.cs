using System.Collections.Generic;
using UnityEngine;

public class RegionsViewModel
{
    private List<RegionViewModel> _regions = new();
    public RegionsViewModel()
    {
        foreach (RegionId id in System.Enum.GetValues(typeof(RegionId))) {
            if (id == RegionId.Unknown) {
                continue;
            }
            _regions.Add(new RegionViewModel(id));
        }
    }
    public bool TryGetRegion(RegionId regionId, out RegionViewModel region)
    {
        foreach(var reg in _regions) {
            if (reg.Id == regionId) {
                region = reg;
                return true;
            }
        }
        Debug.LogWarning($"Can't find {regionId} in regions view models!");
        region = null;
        return false;
    }
    public bool TryGetFreeSpawnPoint(RegionId regionId, Vector3 hitPoint, out SpawnPoint spawnPoint) 
    {
        if (TryGetRegion(regionId, out var region)) {
            if (region.TryGetFreeSpawnPoint(hitPoint, out var sp)) {
                spawnPoint = sp;
                return true;
            }
        }
        spawnPoint = null;
        return false;
    }
}
