using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Board;
using static UnityEngine.Rendering.DebugUI;

public class RegionViewManager : MonoBehaviour
{
    public void SubscribeOnInit(IEnumerable<RegionRuntimeData> runtimeRegions) {
        foreach (var data in runtimeRegions) {
            Transform regionTransform = FindRegionByName(data.RegionStaticData.RegionName);
            regionTransform.GetComponentInChildren<RegionEffectsManager>().SubscribeOnOwnerChanged(data);
        }
        foreach (var data in runtimeRegions) {
            data.OnHopliteCountChanged += SetHopliteCounter;
        }
    }
    public void SetHopliteCounter(RegionId regionId, PlayerColor color, int count) {
        //Debug.Log("Setting hoplite counter!");
        var region = FindRegionById(regionId);
        GameObject hoplite = FindHopliteInRegion(region, color);
        if (hoplite != null) {
            hoplite.GetComponent<TokenView>()?.SetCount(count);
        }
    }
    public void MoveTokenToRegion(RegionId regionId, GameObject gameObject) {
        gameObject.transform.SetParent(FindRegionById(regionId), worldPositionStays: true);
    }
    public void UniteHopliteTokens(RegionId regionId) {
        Transform regionTransform = transform.Find(RegionIdParser.IdToString(regionId));
        if (regionTransform == null) {
            Debug.LogError($"Region '{regionId}' not found under '{name}'!");
            return;
        }
    }
    public GameObject SetHopliteCounter(int value, RegionId regionId, PlayerColor color) {
        var region = FindRegionById(regionId);
        if (region != null) {
            GameObject hoplite = FindHopliteInRegion(region, color);
            if (hoplite != null) {
                TokenView hoplitePrefab = hoplite.GetComponent<TokenView>();
                hoplitePrefab.SetLabel(value.ToString());
                return hoplite;
            }
        }
        Debug.LogWarning("Couldn't set hoplite visuals counter!");
        return null;
    }
    public SpawnPoint PlaceToken(GameObject token, RegionId regionId, Vector3? position) {
        var spawnPoint = GetFreeSpawnPoint(regionId, position);
        if (spawnPoint != null) {
            spawnPoint.Occupy();
            MoveTokenToRegion(regionId, token);

            var tokenPrefabVisual = token.GetComponent<TokenView>();
            if (tokenPrefabVisual != null) {
                tokenPrefabVisual.SpawnPointId = spawnPoint.Id;
            }
            token.transform.position = spawnPoint.Position;
            return spawnPoint;            
        } else {
            Debug.LogError($"No free spawn points in region {regionId}");
        }
        return spawnPoint;
    }

    public void RemoveToken(RegionId regionId, TokenType tokenType, PlayerColor color) {
        var region = FindRegionById(regionId);
        if (region != null) {
            switch (tokenType) {
                case TokenType.Hoplite:
                    GameObject hoplite = FindHopliteInRegion(region, color);
                    if (hoplite != null) {
                        TokenView hoplitePrefab = hoplite.GetComponent<TokenView>();
                        int hopliteCount = hoplitePrefab.GetHopliteCount();
                        if (hopliteCount <= 1) {
                            ReleaseSpawnPoint(regionId, hoplitePrefab.SpawnPointId);
                            Object.Destroy(hoplite);
                        }
                    } else {
                        Debug.LogWarning("No hoplite found in RegionManagerVisuals::RemoveToken!");
                    }
                    break;
                case TokenType.Hero:
                    GameObject hero = FindHeroInRegion(region, color);
                    if (hero != null) {
                        TokenView heroPrefab = hero.GetComponent<TokenView>();
                        ReleaseSpawnPoint(regionId, heroPrefab.SpawnPointId);
                        Object.Destroy(hero);
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown token type in RegionManagerVisuals::RemoveToken!");
                    break;
            }
        }
    }
    public SpawnPoint GetFreeSpawnPoint(RegionId regionId, Vector3? position = null) {
        var region = FindRegionById(regionId);
        if (region != null) {
            foreach (Transform child in region) {
                var generator = child.GetComponent<SpawnPointGenerator>();
                if (generator != null) {
                    return position.HasValue
                        ? generator.GetNearestUnoccupied(position.Value)
                        : generator.GetCenteredUnoccupied();
                }
            }
        }
        return null;
    }

    public GameObject GetHopliteFromRegion(RegionId regionId, PlayerColor color) {
        var region = FindRegionById(regionId);
        if (region != null) {
            foreach (Transform child in region) {
                TokenView visual = child.GetComponent<TokenView>();
                if (visual != null && visual.PlayerColor == color && visual.TokenType == TokenType.Hoplite) {
                    return child.gameObject;
                }
            }
        }
        return null;
    }
    private void ReleaseSpawnPoint(RegionId regionId, int spawnPointId) {
        var region = FindRegionById(regionId);
        if (region != null) {
            foreach (Transform child in region) {
                var generator = child.GetComponent<SpawnPointGenerator>();
                if (generator != null) {
                    generator.ReleaseSpawnPoint(spawnPointId);                    
                }
            }
        }
    }
    private Transform FindRegionById(RegionId regionId) {
        return FindRegionByName(RegionIdParser.IdToString(regionId));
    }
    private Transform FindRegionByName(string regionName) {
        Transform regionTransform = transform.Find(regionName);
        if (regionTransform == null) {
            Debug.LogError($"Region '{regionName}' not found under '{name}'!");
            return null;
        }
        return regionTransform;
    }
    public GameObject FindHopliteInRegion(Transform region, PlayerColor color) {
        foreach (Transform child in region) {
            TokenView visual = child.GetComponent<TokenView>();
            if (visual != null && visual.PlayerColor == color && visual.TokenType == TokenType.Hoplite) {
                return child.gameObject;
            }
        }
        Debug.LogWarning($"No hoplite of color {color} found in region {region.name}");
        return null;
    }
    private GameObject FindHeroInRegion(Transform region, PlayerColor color) {
        foreach (Transform child in region) {
            TokenView visual = child.GetComponent<TokenView>();
            if (visual != null && visual.PlayerColor == color && visual.TokenType == TokenType.Hero) {
                return child.gameObject;
            }
        }
        Debug.LogWarning($"No hero of color {color} found in region {region.name}");
        return null;
    }
}
