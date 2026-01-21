using UnityEngine;

public class RegionsView : MonoBehaviour
{
    private void Awake()
    {
    }

    public void SetHopliteCounter(RegionId regionId, PlayerColor color, int count)
    {
        var region = FindRegionById(regionId);
        GameObject hoplite = FindHopliteInRegion(region, color);
        if (hoplite != null) {
            hoplite.GetComponent<TokenView>()?.SetCount(count);
        }
    }

    public TokenNest PlaceToken(TokenView token, RegionId regionId, Vector3? pos)
    {
        var spawnPoint = GetFreeSpawnPoint(regionId, pos);
        if (spawnPoint != null) {
            PlaceTokenAtSpawn(token, spawnPoint);
            return spawnPoint;
        } else {
            Debug.LogError($"No free spawn points in region {regionId}");
        }
        return spawnPoint;
    }
    public void PlaceTokenAtSpawn(TokenView token, TokenNest spawnPoint)
    {
        spawnPoint.Occupy();
        token.transform.SetParent(FindRegionById(spawnPoint.RegionId), worldPositionStays: true);
        token.SpawnPoint = spawnPoint;
        token.SetPosition(spawnPoint.Position);
    }
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
                            hoplitePrefab.SpawnPoint.Release();
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
                        heroPrefab.SpawnPoint.Release();
                        Destroy(hero);
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown token type in RegionManagerVisuals::RemoveToken!");
                    break;
            }
        }
    }
    public TokenNest GetFreeSpawnPoint(RegionId regionId, Vector3? position = null)
    {
        var region = FindRegionById(regionId);
        if (region != null) {
            foreach (Transform child in region) {
                var spawnPoints = child.GetComponent<TokenNestsView>();
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
