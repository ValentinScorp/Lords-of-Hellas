using UnityEngine;

public class RulesCheckerStartup : IRulesChecker
{
    public bool CanPlace(TokenModel token, RegionNest nest)
    {
        var regions = GameContext.Instance.RegionRegistry;
        if (token is HopliteModel hoplite) {
            if (regions.IsHeroAnotherColor(nest.RegionId, hoplite.PlayerColor)) {
                return false;
            }
            if (regions.IsHopliteAnotherColor(nest.RegionId, hoplite.PlayerColor)) {
                return false;
            }
            if (regions.IsHeroSameColor(nest.RegionId, hoplite.PlayerColor)) {
                return true;
            }
            if (regions.IsHopliteSameColor(hoplite.PlayerColor, nest.RegionId)) {
                return true;
            }
            return !regions.IsAnotherTokenPlacedOnMap(hoplite.PlayerColor, out var _);
        }
        else if (token is HeroModel hero) {
            if (regions.IsHeroAnotherColor(nest.RegionId, hero.PlayerColor)) {
                return false;
            }
            if (regions.IsHopliteAnotherColor(nest.RegionId, hero.PlayerColor)) {
                return false;
            }
            if (regions.IsHopliteSameColor(hero.PlayerColor, nest.RegionId)) {
                return true;
            }
            return !regions.IsAnotherTokenPlacedOnMap(hero.PlayerColor, out var _);
        }
        return false;
    }
}
