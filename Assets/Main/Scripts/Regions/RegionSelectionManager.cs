using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RegionSelectionManager : MonoBehaviour
{
    // [SerializeField] private UserInputController _userInputController;
    private RegionInfoUiPanel _regionInfoUiPanel;

    private InputAction _clickAction;
    private Camera _camera;
    private RegionAreaView _selected;

    private void Awake()
    {
        _camera = Camera.main;
    }
    private void Start()
    {
        _regionInfoUiPanel = ServiceLocator.Get<UiRegistry>().Get<RegionInfoUiPanel>();
    }
    // private void OnClickPerformed(Vector2 screenPosition)
    // {
    //     RegionAreaView target = _userInputController.GetRaycastTarget<RegionAreaView>();
    //     if (target != null) {
    //         Select(target);
    //     } else {
    //         Deselect();
    //     }
    // }
    // private bool IsPointerOverUI(Vector2 screenPosition)
    // {
    //     PointerEventData pointerEventData = new PointerEventData(EventSystem.current) {
    //         position = screenPosition
    //     };

    //     List<RaycastResult> results = new List<RaycastResult>();
    //     EventSystem.current.RaycastAll(pointerEventData, results);

    //     int excludedLayer = LayerMask.NameToLayer("RaycastHitTransparent");

    //     foreach (var result in results) {
    //         if (result.gameObject.layer != excludedLayer) {
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    private void Select(RegionAreaView newTarget)
    {
        if (_selected != null && _selected != newTarget) {
            _selected.Deactivate();
        }

        _selected = newTarget;
        _selected.Activate();
        var regionStatus = ServiceLocator.Get<RegionStatusRegistry>().GetRegionData(_selected.RegionId);

        _regionInfoUiPanel.ShowRegionInfo(regionStatus);
    }

    private void Deselect()
    {
        if (_selected != null) {
            _selected.Deactivate();
            _regionInfoUiPanel.HidePanel();
            _selected = null;
        }
    }

}
