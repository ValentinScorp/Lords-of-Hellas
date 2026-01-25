using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenNestHitDetector
{
    private Action<TokenNest> _onHitDetected;
    public void ListenHits(Action<TokenNest> OnHitDetected)
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
            if (target.Selectable is RegionAreaView regionArea) {
                if (ServiceLocator.Get<RegionsViewModel>().TryGetFreeSpawnPoint(regionArea.Id, target.HitPoint, out var nest)) {
                    _onHitDetected?.Invoke(nest);
                }
            }
        }
    }
}
