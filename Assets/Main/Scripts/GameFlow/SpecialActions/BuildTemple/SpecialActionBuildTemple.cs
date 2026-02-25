using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialActionBuildTemple
{
    private Player _player;
    private List<RegionModel> _regions = new();
    private Action<Player> _Completed;
    internal void Init(Player player)
    {
        _player = player;
        _regions = GameContext.Instance.RegionRegistry.GetRegionsWithEmptyShrines(player);
    }
    internal bool CanExecute()
    {
        if (_player is null || _regions is null) return false;
        return _regions.Count > 0;
    }

    internal void Launch(Action<Player> Completed)
    {
        if (!CanExecute()) {
            Completed?.Invoke(_player);
        }
        _Completed = Completed;
        ServiceLocator.Get<ObjectsHitDetector>().Listen(OnObjectsHit);
    }
    private void OnObjectsHit(List<ObjectsHitDetector.Target> targets)
    {
        foreach (var target in targets) {
            if (target.Hittable is RegionAreaView region) {
                if (CanPlaceTemple(region.RegionId)) {
                    if(!GameContext.Instance.RegionRegistry.TryPlaceTemple(region.RegionId)) {
                        Debug.LogWarning($"Error placing Temple in {region.RegionId}");
                    }
                    ServiceLocator.Get<ObjectsHitDetector>().Unlisten();
                    _player.PlacePriestInPool();
                    _Completed(_player);
                }                    
            }
        }
    }
    private bool CanPlaceTemple(RegionId regionId)
    {
        if (_regions is null) return false;
        foreach (var region in _regions) {
            if (region.RegionId == regionId) {
                return true;
            }
        }
        return false;
    }
}
