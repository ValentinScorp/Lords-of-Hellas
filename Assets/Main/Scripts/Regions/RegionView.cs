using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;

public class RegionView : MonoBehaviour
{
    private RegionId _id = RegionId.Unknown;
    private RegionViewModel _viewModel;
    private RegionAreaView _areaView;
    private RegionBorderView _borderView;

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
            // if (!areaTransform.TryGetComponent(out _spawnPoint)) {
            //     Debug.LogError($"RegionView '{name}' could not get SpawnPoint component from child Area!");
            // }
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
        if (ServiceLocator.Get<RegionsViewModel>().TryGetRegion(_id, out var regionVm)) {
            _viewModel = regionVm;
        }
    }
}
