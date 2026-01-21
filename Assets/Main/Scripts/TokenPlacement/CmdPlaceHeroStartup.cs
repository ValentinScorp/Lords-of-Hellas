using System;
using System.Collections.Generic;
using UnityEngine;

public class CmdPlaceHeroStartup
{
    private HeroModel _hero;
    private ObjectsHitDetector _objectsHitDetector;
    private TokenDragger _tokenDragger;
    private Action<CommandResult> _CommandCompleted; 
    public void Init(HeroModel hero)
    {
        if (_tokenDragger == null) {
            _tokenDragger = new TokenDragger();
        }
        _hero = hero;
        _objectsHitDetector = ServiceLocator.Get<ObjectsHitDetector>();
    }
    public bool CanExecute()
    {
        return !GameContext.Instance.RegionDataRegistry.TryFindHero(_hero, out var regionId);
    }
    public void Execute(Action<CommandResult> CmdComplete)
    {
        if (!CanExecute()) {
            CmdComplete?.Invoke(CommandResult.Fail("Hero already placed."));
            return;
        }
        _CommandCompleted = CmdComplete;
        var _ghostToken = ServiceLocator.Get<TokenFactory>().CreateGhostToken(_hero);
        _tokenDragger.SetTarget(_ghostToken);
       _objectsHitDetector.Listen(HandleHits);
    }
    public void HandleHits(List<ObjectsHitDetector.Target> targets)
    {
        if (targets == null) return;

        foreach (var target in targets){
            if (target.Selectable is RegionAreaView regionArea) {
                RegionHited(regionArea, target.HitPoint);
                break;
            }
        }
    }
    private void RegionHited(RegionAreaView region, Vector3 hitPoint)
    {
        var regRegistry = GameContext.Instance.RegionDataRegistry;
        ServiceLocator.Get<ObjectsHitDetector>().Unlisten();
        if (ServiceLocator.Get<RegionsViewModel>().TryGetFreeSpawnPoint(region.Id, hitPoint, out var spawnPoint)) {
            if (_tokenDragger.TryRemoveTarget(out var token)) {
                token.Place(spawnPoint);
                _objectsHitDetector.Unlisten();
                _CommandCompleted?.Invoke(CommandResult.Ok());
            }
        }
    }
}
