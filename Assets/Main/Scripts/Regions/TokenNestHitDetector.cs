using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenNestHitDetector
{
    private Action<RegionNest> _onHitDetected;
    public void ListenHits(Action<RegionNest> OnHitDetected)
    {
        _onHitDetected = OnHitDetected;

        ServiceLocator.Get<ObjectsHitDetector>().Listen(HandleHits);
    }
    public void Unlisten()
    {
        _onHitDetected = null;
        ServiceLocator.Get<ObjectsHitDetector>().Unlisten();
    }
    private void HandleHits(List<ObjectsHitDetector.Target> targets)
    {
        if (targets == null) return;

        foreach (var target in targets){
            if (target.Hittable is RegionAreaView regionArea) {
                if (ServiceLocator.Get<RegionsView>().TryGetFreeNest(regionArea.Id, target.HitPoint, out var nest)) {
                    _onHitDetected?.Invoke(nest);
                }
            }
        }
    }
}
