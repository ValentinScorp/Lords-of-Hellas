using System;
using System.Collections.Generic;
using UnityEngine;

public class CmdPlaceHeroStartup
{
    private HeroModel _hero;
    private TokenDragger _tokenDragger;
    private TokenNestHitDetector _tokenNestHitDetector = new();
    private Action<CommandResult> _CommandCompleted; 
    public void Init(HeroModel hero)
    {
        if (_tokenDragger == null) {
            _tokenDragger = new TokenDragger();
        }
        _hero = hero;
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
        _tokenNestHitDetector.ListenHits(HandleHitedNest);
    }
    public void HandleHitedNest(TokenNest nest)
    {
         if (_tokenDragger.TryRemoveTarget(out var token)) {
            if (ServiceLocator.Get<RegionsViewModel>().TryRegisterToken(token, nest)) {
                _CommandCompleted?.Invoke(CommandResult.Ok());
            }
        }
    }
}
