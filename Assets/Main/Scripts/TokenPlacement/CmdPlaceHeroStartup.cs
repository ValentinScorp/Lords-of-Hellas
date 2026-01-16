using UnityEngine;

public class CmdPlaceHeroStartup
{
    private HeroModel _hero;
    private RegionsContext _regionsContext;
    public void Init(HeroModel hero)
    {
        _hero = hero;
        _regionsContext = ServiceLocator.Get<RegionsContext>();
    }
    public void Execute()
    {
        Debug.Log("Execution place Hero command!");
        ServiceLocator.Get<TokenPrefabFactory>().CreateGhostToken(_hero);
    }
    public bool CanExecute()
    {
        return _regionsContext.TryFindHero(_hero, out var regionId);
    }
}
