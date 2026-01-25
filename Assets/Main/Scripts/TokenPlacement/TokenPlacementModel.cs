using System;

public sealed class TokenPlacementModel
{
    private Player _player;

    public void StartPlacement(Player player)
    {
        _player = player;
    }
    public HeroModel Hero => _player.Hero;
    public HopliteModel Hoplite => _player.TakeHoplite();
    public bool CanPlaceHero()
    {
        return !GameContext.Instance.RegionDataRegistry.TryFindHero(_player.Hero, out var regionId);
    }
    public bool CanPlaceHoplite()
    {
        return GameContext.Instance.RegionDataRegistry.HoplitesCount(_player.Color) < 2;
    }
    public bool CanComplete()
    {
        return !CanPlaceHero() && !CanPlaceHoplite();
    }
}
