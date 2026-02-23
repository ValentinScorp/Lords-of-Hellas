using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class RegionsContext
{
    private List<RegionContext> _regionsContextList;
    public List<RegionContext> RegionsContextList => _regionsContextList;
    private readonly Dictionary<RegionId, RegionContext> _regionMap = new();

    public RegionsContext(List<RegionConfig> regionConfigs)
    {
        _regionsContextList = new();
        foreach (var regConf in regionConfigs) {
            var regionData = new RegionContext(regConf);            
            _regionsContextList.Add(regionData);
        }
        foreach (var region in _regionsContextList) {
            _regionMap[region.RegionId] = region;
        }

        BuildGraph();
    }

    private void BuildGraph()
    {
        foreach (RegionContext region in _regionsContextList) {
            region.RegionConfig.RegionConnections = new List<RegionConnection>();

            foreach (string neighborName in region.RegionConfig.SourceData.neighbors_land) {
                if (_regionMap.TryGetValue(RegionIdParser.Parse(neighborName), out RegionContext neighbor)) {
                    region.RegionConfig.RegionConnections.Add(new RegionConnection {
                        TargetRegionId = neighbor.RegionId,
                        ConnectionType = RegionConnectionType.Land
                    });
                } else {
                    Debug.LogWarning($"Land neighbor '{neighborName}' not found for region '{region.RegionConfig.RegionName}'");
                }
            }
            foreach (string seaNeighborName in region.RegionConfig.SourceData.neighbors_sea) {
                if (_regionMap.TryGetValue(RegionIdParser.Parse(seaNeighborName), out RegionContext neighbor)) {
                    region.RegionConfig.RegionConnections.Add(new RegionConnection {
                        TargetRegionId = neighbor.RegionId,
                        ConnectionType = RegionConnectionType.Sea
                    });
                } else {
                    Debug.LogWarning($"Sea neighbor '{seaNeighborName}' not found for region '{region.RegionConfig.RegionName}'");
                }
            }
        }
    }
    public bool TryPlaceTemple(RegionId regionId)
    {
        if (TryFindRegion(regionId, out var region)) {
            return region.TryPlaceTemple();
        }
        return false;
    }
    public bool TryPlace(TokenModel token, RegionId regionId)
    {
        if (TryFindRegion(regionId, out var region)) {
            region.Place(token);
            return true;
        }
        return false;
    }
    public bool TryPlace(TokenModel token, RegionNest nest)
    {
        if (TryFindRegion(nest.RegionId, out var region)) {
            region.Place(token, nest);
            return true;
        }
        return false;
    }
    public bool TryTake(TokenModel token, RegionId regionId)
    {
        if (TryFindRegion(regionId, out var region)) {
            region.Take(token);
            return true;
        }
        return false;
    }
    public void Move(TokenModel token, RegionNest nest)
    {
        Debug.Log($"{token.RegionId} to {nest.RegionId}");
        if (TryFindRegion(token.RegionId, out var fromReg) && TryFindRegion(nest.RegionId, out var toReg)) {
            Debug.Log($"{fromReg.RegionId} to {toReg.RegionId}");
            fromReg.Take(token);
            toReg.Place(token, nest);
        }
    }
    public bool TryFindToken(RegionId regionId, TokenType tokenType, PlayerColor playerColor, out TokenModel token)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.TryGetToken(tokenType, playerColor, out token);
        }
        token = null;
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool TryFindHero(HeroModel hero, out RegionId regionId)
    {
        if (hero == null) {
            regionId = RegionId.Unknown;
            return false;
        }
        if (hero.RegionId != RegionId.Unknown &&
            _regionMap.TryGetValue(hero.RegionId, out var knownRegion)) {
            foreach (var token in knownRegion.Tokens) {
                if (ReferenceEquals(token, hero)) {
                    regionId = knownRegion.RegionId;
                    return true;
                }
            }
        }
        foreach (var region in _regionMap.Values) {
            foreach (var token in region.Tokens) {
                if (ReferenceEquals(token, hero)) {
                    regionId = region.RegionId;
                    return true;
                }
            }
        }
        regionId = RegionId.Unknown;
        return false;
    }
    public int HoplitesCount(PlayerColor color)
    {
        int total = 0;
        foreach (var region in _regionsContextList) {
            total += region.GetHopliteCount(color);
        }
        return total;
    }

    public bool TryMoveHoplite(HopliteModel hoplite, RegionId regionId)
    {
        if (hoplite == null) {
            return false;
        }
        RegionContext fromRegion = GetRegionContext(hoplite.RegionId);
        RegionContext toRegion = GetRegionContext(regionId);

        if (fromRegion == null || toRegion == null) {
            return false;
        }
        fromRegion.Take(hoplite);
        toRegion.Place(hoplite);
        hoplite.MarkMoved();
        return true;    
    }
    public bool TryFindRegion(RegionId regionId, out RegionContext region)
    {
        region = GetRegionContext(regionId);
        if (region is not null) {
            return true;
        }
        return false;
    }
    public RegionContext GetRegionContext(RegionId regionId)
    {
        if (_regionMap.TryGetValue(regionId, out RegionContext regionData)) {
            return regionData;
        }
        Debug.LogWarning($"Region with ID {regionId} not found in _regionMap");
        return null;
    }
    public bool IsTokenInRegion(TokenType tokenType, RegionId regionId)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsToken(tokenType);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsHopliteSameColor(PlayerColor color, RegionId regionId)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsHopliteSameColor(color, out var hoplite);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsHeroAnotherColor(RegionId regionId, PlayerColor color)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsHeroAnotherColor(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsHeroSameColor(RegionId regionId, PlayerColor color)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsHeroSameColor(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsHopliteAnotherColor(RegionId regionId, PlayerColor color)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsHopliteAnotherColor(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsAnotherTokenPlacedOnMap(PlayerColor color, out RegionId regionId)
    {
        foreach (var region in _regionMap.Values) {
            if (region.ContainsToken(color)) {
                regionId = region.RegionId;
                return true;
            }
        }
        regionId = RegionId.Unknown;
        return false;
    }
    public int GetHopliteNum(RegionId regionId, PlayerColor color)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.GetHopliteCount(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return 0;
    }

    public bool TryTake(RegionId regionId, TokenType tokenType, PlayerColor color, out TokenModel token)
    {
        token = null;
        Debug.Log("Unregistering token " + tokenType.ToString());
        var region = GetRegionContext(regionId);
        if (region == null) {
            Debug.LogWarning($"Region {regionId} not found for unregistering token.");            
            return false;
        }
        if (region.TryFindToken(tokenType, color, out TokenModel t)) {
            region.Take(t);
            GameLogger.Instance.Event($"Removing {tokenType} ({color}) from {regionId}");
            token = t;
            return true;
        }
        Debug.LogWarning("Unable to unregister token in RegionManager!");
        return false;
    }
    public List<RegionId> GetControlled(Player player)
    {
        List<RegionId> regionIds = new();
        foreach (var r in _regionsContextList) {
            if (r.OwnedBy == player.Color) {
                regionIds.Add(r.RegionId);
            }
        }
        return regionIds;
    }
    public List<RegionId> GetNeighborRegionIds(RegionId regionId)
    {
        List<RegionId> regionIds = new();
        if (TryGetNeighborRegions(regionId, out var neighbors)) {
            foreach (var rrd in neighbors) {
                regionIds.Add(rrd.RegionId);
            }
        }
        return regionIds;
    }
    private bool TryGetNeighborRegions(RegionId regionId, out List<RegionContext> neighbors)
    {
        neighbors = new List<RegionContext>();
        if (!_regionMap.TryGetValue(regionId, out var region)) {
            Debug.LogWarning($"Region {regionId} not found in map.");
            return false;
        }
        if (region.RegionConfig.RegionConnections == null) {
            Debug.LogWarning($"Region {regionId} neighbors are not initialized.");
            return false;
        }
        foreach (var rc in region.RegionConfig.RegionConnections) {
            if (_regionMap.TryGetValue(rc.TargetRegionId, out var neighborRegion)) {
                neighbors.Add(neighborRegion);
            } else {
                Debug.LogWarning($"Neighbor region {rc.TargetRegionId} not found in map.");
            }
        }
        return true;
    }
}
