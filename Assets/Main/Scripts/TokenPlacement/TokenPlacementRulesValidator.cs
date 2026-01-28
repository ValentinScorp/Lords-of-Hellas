using UnityEngine;

public class TokenPlacementRulesValidator
{
    public bool ValidateLogicalPlacement(RegionsContext regionManager, RegionId regionId, TokenModel token) {
        var regionData = regionManager.GetRegionContext(regionId);
        if (regionData == null) {
            Debug.Log("Region data not found.");
            return false;
        }
        if (regionManager.IsHeroAnotherColor(regionId, token.PlayerColor)) {
            Debug.Log("There is Hero of another player in this region.");
            return false;
        }
        if (regionManager.IsHopliteSameColor(token.PlayerColor, regionId)) {
            Debug.Log("There is hoplite of another player in this region.");
            return false;
        }
        if (regionManager.IsAnotherTokenPlacedOnMap(token.PlayerColor, out RegionId id)) {
            if (regionId != id) {
                Debug.Log("There is of same color token placed already in another region!" + RegionIdParser.IdToString(id));
                return false;
            }
        }

        return true;
    }
}
