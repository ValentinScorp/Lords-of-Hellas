using UnityEngine;

public class RegionsView : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<RegionsView>();     
    }

    public void SetHopliteCounter(RegionId regionId, PlayerColor color, int count)
    {
        var region = FindRegionById(regionId);
        GameObject hoplite = FindHopliteInRegion(region, color);
        if (hoplite != null) {
            hoplite.GetComponent<TokenView>()?.SetCount(count);
        }
    }

    // public TokenNest PlaceToken(TokenView token, RegionId regionId, Vector3? pos)
    // {
    //     var spawnPoint = GetFreeSpawnPoint(regionId, pos);
    //     if (spawnPoint != null) {
    //         PlaceTokenAtSpawn(token, spawnPoint);
    //         return spawnPoint;
    //     } else {
    //         Debug.LogError($"No free spawn points in region {regionId}");
    //     }
    //     return spawnPoint;
    // }
    // public void PlaceTokenAtSpawn(TokenView token, TokenNest spawnPoint)
    // {
    //     spawnPoint.Occupy();
    //     token.transform.SetParent(FindRegionById(spawnPoint.RegionId), worldPositionStays: true);
    //     token.Nest = spawnPoint;
    //     token.SetPosition(spawnPoint.Position);
    // }
    public void RemoveToken(RegionId regionId, TokenType tokenType, PlayerColor color)
    {
        var region = FindRegionById(regionId);
        if (region != null) {
            switch (tokenType) {
                case TokenType.HopliteStack:
                    GameObject hoplite = FindHopliteInRegion(region, color);
                    if (hoplite != null) {
                        TokenView hoplitePrefab = hoplite.GetComponent<TokenView>();
                        int hopliteCount = hoplitePrefab.GetHopliteCount();
                        if (hopliteCount <= 1) {
                            hoplitePrefab.Nest.Release();
                            Destroy(hoplite);
                        }
                    } else {
                        Debug.LogWarning("No hoplite found in RegionManagerVisuals::RemoveToken!");
                    }
                    break;
                case TokenType.Hero:
                    GameObject hero = FindHeroInRegion(region, color);
                    if (hero != null) {
                        TokenView heroPrefab = hero.GetComponent<TokenView>();
                        heroPrefab.Nest.Release();
                        Destroy(hero);
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown token type in RegionManagerVisuals::RemoveToken!");
                    break;
            }
        }
    }
    public bool TryGetNest(RegionId regionId, int nestId, out RegionNest nest)
    {
        var region = FindRegionById(regionId);
        if (region != null) {
            foreach (Transform child in region) {
                var nestsView = child.GetComponent<RegionNestsView>();
                if (nestsView != null) {
                    nest = nestsView.GetNest(nestId);
                    if (nest is not null) {
                        return true;
                    }
                }
            }
        }
        nest = null;
        return false;
    }
    public bool TryGetFreeNest(RegionId regionId, Vector3 position, out RegionNest nest)
    {
        nest = GetFreeNest(regionId, position);
        if (nest is not null) return true;
        return false;
    }
    public RegionNest GetFreeNest(RegionId regionId, Vector3? position = null)
    {
        var region = FindRegionById(regionId);
        if (region != null) {
            foreach (Transform child in region) {
                var spawnPoints = child.GetComponent<RegionNestsView>();
                if (spawnPoints != null) {
                    return position.HasValue ? 
                        spawnPoints.GetNearestUnoccupied(position.Value) :
                        spawnPoints.GetCenteredUnoccupied();
                }
            }
        }
        return null;
    }

    public GameObject GetHopliteFromRegion(RegionId regionId, PlayerColor color)
    {
        var region = FindRegionById(regionId);
        if (region != null) {
            foreach (Transform child in region) {
                TokenView visual = child.GetComponent<TokenView>();
                if (visual != null && visual.PlayerColor == color && visual.TokenType == TokenType.HopliteStack) {
                    return child.gameObject;
                }
            }
        }
        return null;
    }
    private Transform FindRegionById(RegionId regionId)
    {
        return FindRegionByName(RegionIdParser.IdToString(regionId));
    }
    private Transform FindRegionByName(string regionName)
    {
        Transform regionTransform = transform.Find(regionName);
        if (regionTransform == null) {
            Debug.LogError($"Region '{regionName}' not found under '{name}'!");
            return null;
        }
        return regionTransform;
    }
    public GameObject FindHopliteInRegion(Transform region, PlayerColor color)
    {
        foreach (Transform child in region) {
            TokenView visual = child.GetComponent<TokenView>();
            if (visual != null && visual.PlayerColor == color && visual.TokenType == TokenType.HopliteStack) {
                return child.gameObject;
            }
        }
        Debug.LogWarning($"No hoplite of color {color} found in region {region.name}");
        return null;
    }
    private GameObject FindHeroInRegion(Transform region, PlayerColor color)
    {
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
