using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RegionDataManager
{
    private readonly Dictionary<RegionId, RegionRuntimeData> _regionMap = new();
    private RegionStaticDataLoader _mapLoader = new();

    public RegionDataManager(RegionViewManager regionManagerVisuals) {
        _mapLoader.LoadMapData();

        foreach (var regionData in GameData.Instance.RegionStaticData) {
            RegionRuntimeData regionRuntimeData = new(regionData);
            GameState.Instance.RegionRuntimeData.Add(regionRuntimeData);
        }
        foreach (var region in GameState.Instance.RegionRuntimeData) {
            _regionMap[region.RegionId] = region;
        }

        BuildGraph();

        regionManagerVisuals.SubscribeOnInit(_regionMap.Values.ToList());
    }
    
    private void BuildGraph() {
        foreach (RegionRuntimeData region in GameState.Instance.RegionRuntimeData) {
            region.RegionStaticData.Neighbors = new List<RegionConnection>();

            foreach (string neighborName in region.RegionStaticData.SourceData.neighbors_land) {
                if (_regionMap.TryGetValue(RegionIdParser.Parse(neighborName), out RegionRuntimeData neighbor)) {
                    region.RegionStaticData.Neighbors.Add(new RegionConnection {
                        TargetRegionId = neighbor.RegionId,
                        ConnectionType = RegionConnectionType.Land
                    });
                } else {
                    Debug.LogWarning($"Land neighbor '{neighborName}' not found for region '{region.RegionStaticData.RegionName}'");
                }
            }
            foreach (string seaNeighborName in region.RegionStaticData.SourceData.neighbors_sea) {
                if (_regionMap.TryGetValue(RegionIdParser.Parse(seaNeighborName), out RegionRuntimeData neighbor)) {
                    region.RegionStaticData.Neighbors.Add(new RegionConnection {
                        TargetRegionId = neighbor.RegionId,
                        ConnectionType = RegionConnectionType.Sea
                    });
                } else {
                    Debug.LogWarning($"Sea neighbor '{seaNeighborName}' not found for region '{region.RegionStaticData.RegionName}'");
                }
            }
        }
    }
    
    public RegionRuntimeData GetRegionData(RegionId regionId) {
        if (_regionMap.TryGetValue(regionId, out RegionRuntimeData regionData)) {
            return regionData;
        }
        Debug.LogWarning($"Region with ID {regionId} not found in _regionMap");
        return null;
    }
    public bool IsTokenInRegion(TokenType tokenType, RegionId regionId) {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsToken(tokenType);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsHopliteInRegion(PlayerColor color, RegionId regionId) {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsAnotherHopliteOfColor(color, out var hoplite);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsAnotherHeroInRegion(RegionId regionId, PlayerColor color) {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsAnotherHero(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsAnotherHopliteInRegion(RegionId regionId, PlayerColor color) {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.ContainsAnotherHoplite(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return false;
    }
    public bool IsAnotherTokenPlacedOnMap(PlayerColor color, out RegionId regionId) {
        foreach (var region in _regionMap.Values) {
            if (region.ContainsToken(color)) {
                regionId = region.RegionId;
                return true;
            }
        }
        regionId = RegionId.Unknown;
        return false;
    }
    public int GetHopliteNum(RegionId regionId, PlayerColor color) {
        if (_regionMap.TryGetValue(regionId, out var region)) {
            return region.GetHopliteCount(color);
        }
        Debug.LogWarning($"Region {regionId} not found in map.");
        return 0;
    }
    public bool RegisterToken(RegionId regionId, Token token) {
        var region = GetRegionData(regionId);
        if (region == null) {
            Debug.LogWarning($"Region {regionId} not found for registering token.");
            return false;
        }
        region.RegisterToken(token);
        GameLogger.Instance.Event($"Adding {token.Type} to {regionId}");
        return true;
    }

    public bool UnregisterToken(RegionId regionId, TokenType tokenType, PlayerColor color) {
        Debug.Log("Unregistering token " + tokenType.ToString());
        var region = GetRegionData(regionId);
        if (region == null) {
            Debug.LogWarning($"Region {regionId} not found for unregistering token.");
            return false;
        }

        if (region.FindToken(tokenType, color, out Token token)) {
            region.RemoveToken(token);
            GameLogger.Instance.Event($"Removing {tokenType} ({color}) from {regionId}");
            return true;
        }

        Debug.LogWarning("Unable to unregister token in RegionManager!");
        return false;       
    }
}
