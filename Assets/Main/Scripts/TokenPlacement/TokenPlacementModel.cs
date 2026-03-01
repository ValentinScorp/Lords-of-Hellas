using System;

internal sealed class TokenPlacementModel
{
    private Player _player;

    internal void SetPlayer(Player player)
    {
        _player = player;
    }
    internal HeroModel Hero => _player.Hero;
    internal HopliteModel Hoplite => _player.TakeHoplite();
    internal bool CanPlaceHero()
    {
        return !GameContext.Instance.RegionRegistry.TryFindHero(_player.Hero, out var regionId);
    }
    internal bool CanPlaceHoplite()
    {
        return GameContext.Instance.RegionRegistry.HoplitesCount(_player.Color) < 2;
    }
    internal bool CanComplete()
    {
        return !CanPlaceHero() && !CanPlaceHoplite();
    }
}
