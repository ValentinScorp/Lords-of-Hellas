using System.Collections.Generic;
using UnityEngine;

public class RegionDataRegistry
{
    private readonly Dictionary<RegionId, RegionData> _regionMap = new();
    private RegionStaticDataLoader _mapLoader = new();

    public RegionDataRegistry()
    {
        _mapLoader.LoadMapData();

        foreach (var regionData in GameData.Instance.RegionStaticData) {
            RegionData regionStatus = new(regionData);
            GameState.Instance.RegionStatuses.Add(regionStatus);
        }
        foreach (var region in GameState.Instance.RegionStatuses) {
            _regionMap[region.RegionId] = region;
        }

        BuildGraph();
    }

    private void BuildGraph()
    {
        foreach (RegionData region in GameState.Instance.RegionStatuses) {
            region.RegionStaticData.RegionConnections = new List<RegionConnection>();

            foreach (string neighborName in region.RegionStaticData.SourceData.neighbors_land) {
                if (_regionMap.TryGetValue(RegionIdParser.Parse(neighborName), out RegionData neighbor)) {
                    region.RegionStaticData.RegionConnections.Add(new RegionConnection {
                        TargetRegionId = neighbor.RegionId,
                        ConnectionType = RegionConnectionType.Land
                    });
                } else {
                    Debug.LogWarning($"Land neighbor '{neighborName}' not found for region '{region.RegionStaticData.RegionName}'");
                }
            }
            foreach (string seaNeighborName in region.RegionStaticData.SourceData.neighbors_sea) {
                if (_regionMap.TryGetValue(RegionIdParser.Parse(seaNeighborName), out RegionData neighbor)) {
                    region.RegionStaticData.RegionConnections.Add(new RegionConnection {
                        TargetRegionId = neighbor.RegionId,
                        ConnectionType = RegionConnectionType.Sea
                    });
                } else {
                    Debug.LogWarning($"Sea neighbor '{seaNeighborName}' not found for region '{region.RegionStaticData.RegionName}'");
                }
            }
        }
    }

    public RegionData GetRegionData(RegionId regionId)
    {
        if (_regionMap.TryGetValue(regionId, out RegionData regionData)) {
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
        var region = GetRegionData(regionId);
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
        var region = GetRegionData(regionId);
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
    private bool TryGetNeighborRegions(RegionId regionId, out List<RegionData> neighbors)
    {
        neighbors = new List<RegionData>();
        if (!_regionMap.TryGetValue(regionId, out var region)) {
            Debug.LogWarning($"Region {regionId} not found in map.");
            return false;
        }
        if (region.RegionStaticData.RegionConnections == null) {
            Debug.LogWarning($"Region {regionId} neighbors are not initialized.");
            return false;
        }
        foreach (var rc in region.RegionStaticData.RegionConnections) {
            if (_regionMap.TryGetValue(rc.TargetRegionId, out var neighborRegion)) {
                neighbors.Add(neighborRegion);
            } else {
                Debug.LogWarning($"Neighbor region {rc.TargetRegionId} not found in map.");
            }
        }
        return true;
    }
}
