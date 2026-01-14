using UnityEngine;

public class RegionView : MonoBehaviour
{
    private RegionId _regionId = RegionId.Unknown;
    private RegionViewController _regionViewController;
    private RegionContext _regionContext;
    private RegionAreaView _areaView;
    private RegionBorderView _borderView;
    private SpawnPointsView _spawnPoint;

    private void Awake()
    {
        _regionId = RegionIdParser.Parse(gameObject.name);

        _regionViewController = new(_regionId);

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
        _regionContext = GameContext.Instance.RegionDataRegistry.GetRegionContext(_regionId);
        if (_regionContext == null) {
            Debug.LogError($"RegionView '{name}' could not get RegionData for RegionId {_regionId}");
            return;
        }
        _regionContext.OnOwnerChanged += _areaView.OnOwnerChanged;
        _regionContext.OnTokenAdded += OnTokenPlaced;
        _regionContext.OnTokenRemoved += OnTokenRemoved;
    }
    private void OnDestroy()
    {
        if (_regionContext != null) {
            _regionContext.OnOwnerChanged -= _areaView.OnOwnerChanged;
            _regionContext.OnTokenAdded -= OnTokenPlaced;
            _regionContext.OnTokenRemoved -= OnTokenRemoved;
        }
    }
    private void OnTokenPlaced(TokenModel token)
    {
        var tokenPrefabFactory = ServiceLocator.Get<TokenPrefabFactory>();
        TokenView tokenView = tokenPrefabFactory.CreateTokenView(token, transform);

        tokenView.AdjustPositionToSpawnPoint();
        tokenView.ChangeToPlayerMaterial();
        tokenView.SetLayer("HoplonToken");
        tokenView.SetTag("PlacedToken");

        if (tokenView.gameObject.TryGetComponent<Rigidbody>(out var rb)) {
            Destroy(rb);
        } else {
            Debug.LogWarning($"No Rigidbody found in: {tokenView.gameObject.name}");
        }
    }
    private void OnTokenRemoved(TokenModel token)
    {
        // TODO
    }
}
