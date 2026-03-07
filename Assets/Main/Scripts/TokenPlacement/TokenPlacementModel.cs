using System;

internal sealed class TokenPlacementModel
{
    public Player Player { set; get; }

    internal HeroModel Hero => Player.Hero;
    internal HopliteModel Hoplite => Player.TakeHoplite();
    internal bool CanPlaceHero()
    {
        return !GameContext.Instance.RegionRegistry.TryFindHero(Player.Hero, out var regionId);
    }
    internal bool CanPlaceHoplite()
    {
        return GameContext.Instance.RegionRegistry.HoplitesCount(Player.Color) < 2;
    }
    internal bool CanComplete()
    {
        return !CanPlaceHero() && !CanPlaceHoplite();
    }
}
