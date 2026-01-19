using System;
using UnityEngine;

public class CmdPlaceHeroStartup
{
    private HeroModel _hero;
    private RegionsContext _regionsContext;
    private TokenDragger _tokenDragger;
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
    public void Execute(Action<CmdResult> CmdComplete)
    {
        if (!CanExecute()) {
            CmdComplete?.Invoke(CmdResult.Fail("Hero already placed."));
            return;
        }

        var _ghostToken = ServiceLocator.Get<TokenFactory>().CreateGhostToken(_hero);
        _tokenDragger.SetTarget(_ghostToken);
        // _ghostToken.SetPlacedVisual();

        // CmdComplete?.Invoke(CmdResult.Ok());
    }
}
