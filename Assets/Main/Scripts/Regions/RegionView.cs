using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;

public class RegionView : MonoBehaviour
{
    private RegionId _id = RegionId.Unknown;
    private RegionContext _region;
    private RegionAreaView _areaView;
    private RegionBorderView _borderView;
    private RegionNestsView _regionNests;
    private List<IPlaceableView> _placeables = new();

    private void Awake()
    {
        _id = RegionIdParser.Parse(gameObject.name);

        if (_id == RegionId.Unknown) {
            Debug.LogWarning($"RegionView '{name}' has unknown RegionId");
        }
        Transform areaTransform = transform.Find("Area");
        if (areaTransform == null) {
            Debug.LogError($"RegionView '{name}' could not find cild Area!");
        } else {
            if (!areaTransform.TryGetComponent(out _areaView)) {
                Debug.LogError($"RegionView '{name}' could not get RegionAreaView component from child Area!");
            }
            if (!areaTransform.TryGetComponent(out _regionNests)) {
                Debug.LogError($"RegionView '{name}' could not get {_regionNests.GetType().Name} component from child Area!");                
            }
        }
        Transform borderTransform = transform.Find("Border");
        if (borderTransform == null || !borderTransform.TryGetComponent(out _borderView)) {
            Debug.LogError($"RegionView '{name}' could not find RegionBorderView under child 'Border'");
        }

        if (_areaView is not null) {
            _areaView.Id = _id;
        }
        
    }
    private void Start()
    {
        _region = GameContext.Instance.RegionRegistry.GetRegionContext(_id);
        if (_region is null) {
            Debug.LogWarning($"Can't get region context of : {_id}!");
        } else {
            _region.TemplePlaced += OnTemplePlaced;
            if (_areaView is not null) {
                _region.OwnerChanged += _areaView.OnOwnerChanged;
            }            
        }
    }
    private void OnDestroy()
    {
        if (_region is not null) {
            _region.TemplePlaced -= OnTemplePlaced;
        }

        if (_region is not null && _areaView is not null) {
            _region.OwnerChanged -= _areaView.OnOwnerChanged;            
        }
    }
    private void OnTemplePlaced(RegionContext region)
    {
         if (region == null || region.RegionId != _id) {
            Debug.LogWarning($"TemplePlaced wrong region for {name}");
            return;
        }
        foreach (var p in _placeables) {
            if (p is TempleView) {
                Debug.LogWarning($"Unable to place second temple in {_id}");
                return;
            }
        }
        if (GameContent.PrefabCatalog == null || GameContent.PrefabCatalog.TemplePrefab == null) {
            Debug.LogError("PrefabCatalog or TemplePrefab is not configured.");
            return;
        }
        if (_regionNests is not null) {
            var templeNest = _regionNests.GetCenteredUnoccupied();
            if (templeNest == null) {
            Debug.LogWarning($"RegionView '{name}' has no temple nest assigned.");
            return;
        }

        TempleView templeView = Instantiate(
            GameContent.PrefabCatalog.TemplePrefab, templeNest.Position, Quaternion.identity, transform);

        templeView.SetOwnerColor(region.OwnedBy);
        _placeables.Add(templeView);
        }        
    }
}
