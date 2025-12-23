using UnityEngine;

public class RegionView : MonoBehaviour
{
    private RegionId _regionId = RegionId.Unknown;
    private RegionData _regionData;
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
            if (!areaTransform.TryGetComponent(out _areaView)) {
                Debug.LogError($"RegionView '{name}' could not get RegionAreaView component from child Area!");
            }
            if (!areaTransform.TryGetComponent(out _spawnPoint)) {
                Debug.LogError($"RegionView '{name}' could not get SpawnPoint component from child Area!");
            }
        }
        Transform borderTransform = transform.Find("Border");
        if (borderTransform == null || !borderTransform.TryGetComponent(out _borderView)) {
            Debug.LogError($"RegionView '{name}' could not find RegionBorderView under child 'Border'");
        }
    }
    private void Start()
    {
        _regionData = ServiceLocator.Get<RegionDataRegistry>().GetRegionData(_regionId);
        if (_regionData == null) {
            Debug.LogError($"RegionView '{name}' could not get RegionData for RegionId {_regionId}");
            return;
        }
        _regionData.OnOwnerChanged += _areaView.OnOwnerChanged;
    }
    private void OnDestroy()
    {
        if (_regionData != null) {
            _regionData.OnOwnerChanged -= _areaView.OnOwnerChanged;
        }
    }
}
