using System.Collections.Generic;
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
    public bool TryGetToken(RegionId regionId, TokenType tokenType, PlayerColor playerColor, out TokenModel token)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.TryGetToken(tokenType, playerColor, out token);
        }
        token = null;
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool MoveHopliteUnit(HopliteModel hopliteUnit, RegionId regionId)
    {
        if (hopliteUnit == null) {
            return false;
        }
        RegionContext fromRegion = GetRegionContext(hopliteUnit.RegionId);
        RegionContext toRegion = GetRegionContext(regionId);

        if (fromRegion == null || toRegion == null) {
            return false;
        }
        if (fromRegion.UnregisterHopliteUnit(hopliteUnit)) {            
            toRegion.RegisterHopliteUnit(hopliteUnit);
            hopliteUnit.ChangeRegion(regionId);
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
    public bool IsHopliteInRegion(PlayerColor color, RegionId regionId)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsAnotherHopliteOfColor(color, out var hoplite);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsAnotherHeroInRegion(RegionId regionId, PlayerColor color)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsAnotherHero(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsAnotherHopliteInRegion(RegionId regionId, PlayerColor color)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsAnotherHoplite(color);
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
    public List<HopliteStackModel> GetHopliteStacks(PlayerColor playerColor)
    {
        List<HopliteStackModel> hopliteStacks = new();
        foreach (var region in _regionsContextList) {
            foreach (var token in region.Tokens) {
                if (token is HopliteStackModel stack && stack.PlayerColor == playerColor) {
                    hopliteStacks.Add(stack);
                }
            }
        }
        return hopliteStacks;
    }
    public int GetHopliteNum(RegionId regionId, PlayerColor color)
    {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.GetHopliteCount(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return 0;
    }
    
    public bool RegisterToken(RegionId regionId, TokenModel token)
    {
        var region = GetRegionContext(regionId);
        if (region == null) {
            Debug.LogWarning($"Region {regionId} not found for registering token.");
            return false;
        }
        region.RegisterToken(token);
        GameLogger.Instance.Event($"Adding {token.Type} to {regionId}");
        return true;
    }

    public bool UnregisterToken(RegionId regionId, TokenType tokenType, PlayerColor color)
    {
        Debug.Log("Unregistering token " + tokenType.ToString());
        var region = GetRegionContext(regionId);
        if (region == null) {
            Debug.LogWarning($"Region {regionId} not found for unregistering token.");
            return false;
        }
        if (region.FindToken(tokenType, color, out TokenModel token)) {
            region.RemoveToken(token);
            GameLogger.Instance.Event($"Removing {tokenType} ({color}) from {regionId}");
            return true;
        }
        Debug.LogWarning("Unable to unregister token in RegionManager!");
        return false;
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
