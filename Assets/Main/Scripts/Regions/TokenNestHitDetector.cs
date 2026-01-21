using System;
using System.Collections.Generic;

public class TokenNestHitDetector
{
    private Action<TokenNest> _onHitDetected;
    public void ListenHits(Action<TokenNest> OnHitDetected)
    {
        _onHitDetected = OnHitDetected;

        ServiceLocator.Get<ObjectsHitDetector>().Listen(HandleHits);
    }
    private void HandleHits(List<ObjectsHitDetector.Target> targets)
    {
        if (targets == null) return;

        foreach (var target in targets){
            if (target.Selectable is RegionAreaView regionArea) {
                if (ServiceLocator.Get<RegionsViewModel>().TryGetFreeSpawnPoint(regionArea.Id, target.HitPoint, out var nest)) {
                    ServiceLocator.Get<ObjectsHitDetector>().Unlisten();                    
                    _onHitDetected?.Invoke(nest);
                    _onHitDetected = null;
                }
            }
        }
    }
}
