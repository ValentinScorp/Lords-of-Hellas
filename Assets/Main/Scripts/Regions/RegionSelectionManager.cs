using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
using static UnityEngine.GraphicsBuffer;

public class RegionSelectionManager : MonoBehaviour
{
    [SerializeField] private RegionInfoUiPanel _regionInfoUiPanel;
    [SerializeField] private UserInputController _userInputController;
    [SerializeField] private GameInitializer _gameInitializer;

    private RegionDataManager _regionManager;

    private InputAction _clickAction;
    private Camera _camera;
    private RegionEffectsManager _selected;

    private void Awake() {
        _camera = Camera.main;
    }
    private void Start() {
        _userInputController.OnMouseClick += OnClickPerformed;
    }
    private void OnDestroy() {
        _userInputController.OnMouseClick -= OnClickPerformed;
    }
    private void OnClickPerformed(InputAction.CallbackContext ctx) {
        if (!_userInputController.IsPointerOverUINoTransparent()) {
            RegionEffectsManager target = _userInputController.GetRaycastTarget<RegionEffectsManager>();
            if (target != null) {
                Select(target);
            } else {
                Deselect();
            }
        }        
    }
    private bool IsPointerOverUI(Vector2 screenPosition) {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current) {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        int excludedLayer = LayerMask.NameToLayer("RaycastHitTransparent");

        foreach (var result in results) {
            if (result.gameObject.layer != excludedLayer) {
                return true;
            }
        }

        return false;
    }

    private void Select(RegionEffectsManager newTarget) {
        if (_selected != null && _selected != newTarget) { 
            _selected.Deactivate();
        }

        _selected = newTarget;
        _selected.Activate();
        RegionRuntimeData regionRuntimeData = _gameInitializer.RegionModelManager.GetRegionData(_selected.RegionId);
        _regionInfoUiPanel.ShowRegionInfo(regionRuntimeData);
    }

    private void Deselect() {
        if (_selected != null) {
            _selected.Deactivate();
            _regionInfoUiPanel.HidePanel();
            _selected = null;
        }
    }

}
