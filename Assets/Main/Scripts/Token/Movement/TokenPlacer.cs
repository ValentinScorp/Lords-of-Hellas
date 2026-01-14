
using UnityEngine;

public class TokenPlacer
{
    public TokenPlacer()
    {
        
    }
    public bool CanPlaceStartup(TokenModel token, RegionId regionId) {        
        if (!ValidateStartupPlacement(ServiceLocator.Get<RegionsContext>(), regionId, token)) {
            return false;
        }
        return true;
    }
    public bool ValidateStartupPlacement(RegionsContext regionManager, RegionId regionId, TokenModel token) {
        var regionData = regionManager.GetRegionContext(regionId);
        if (regionData == null) {
            Debug.Log("Region data not found.");
            return false;
        }
        if (token is IPlayerOwned playerToken) {
            if (regionManager.IsAnotherHeroInRegion(regionId, playerToken.PlayerColor)) {
                Debug.Log("There is Hero of another player in this region.");
                return false;
            }
            if (regionManager.IsAnotherHopliteInRegion(regionId, playerToken.PlayerColor)) {
                Debug.Log("There is hoplite of another player in this region.");
                return false;
            }
            if (regionManager.IsAnotherTokenPlacedOnMap(playerToken.PlayerColor, out RegionId id)) {
                if (regionId != id) {
                    Debug.Log("There is of same color token placed already in another region!" + RegionIdParser.IdToString(id));
                    return false;
                }
            }
        }
        return true;
    }
    public bool ValidatePlacement(RegionsContext regionManager, RegionId regionId, TokenModel token) {
        var regionData = regionManager.GetRegionContext(regionId);
        if (regionData == null) {
            Debug.Log("Region data not found.");
            return false;
        }
        if (token is IPlayerOwned playerToken) {           
            if (token is HopliteStackModel hoplite) {
                var neighbors = regionManager.GetNeighborRegionIds(hoplite.RegionId);
                if (!neighbors.Contains(regionId)) {
                    Debug.Log("Hoplite can move only to a neighboring region.");
                    return false;
                }
            }
        }
        return true;
    }
}
