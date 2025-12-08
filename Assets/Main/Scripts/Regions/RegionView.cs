using UnityEngine;

public class RegionView : MonoBehaviour
{
    [SerializeField] private RegionId _regionId = RegionId.Unknown;
    private RegionAreaView _areaView;
    private RegionBorderView _borderView;
    private SpawnPointsView _spawnPoint;

    private void Awake()
    {
        _regionId = RegionIdParser.Parse(gameObject.name);

        if (_regionId == RegionId.Unknown) {
            Debug.LogWarning($"RegionView '{name}' has unknown RegionId");
        }
        Transform areaTransform = transform.Find("Area");
        if (areaTransform == null) {
            Debug.LogError($"RegionView '{name}' could not find cild Area!");            
        } else {
            if(!areaTransform.TryGetComponent(out _areaView)) {
                Debug.LogError($"RegionView '{name}' could not get RegionAreaView component from child Area!");
            }
            if(!areaTransform.TryGetComponent(out _spawnPoint)) {
                Debug.LogError($"RegionView '{name}' could not get SpawnPoint component from child Area!");
            }
        }

        Transform borderTransform = transform.Find("Border");
        if (borderTransform == null || !borderTransform.TryGetComponent(out _borderView)) {
            Debug.LogError($"RegionView '{name}' could not find RegionBorderView under child 'Border'");
        }
        
    }
}
