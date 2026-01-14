using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RaycastIntersector
{
    private readonly Camera _mainCamera;
    private readonly int _boardLayerMask;
    private readonly GameObject _boardSurface;

    public RaycastIntersector(Camera mainCamera, GameObject boardSurface, int boardLayerMask) {
        _mainCamera = mainCamera;
        _boardSurface = boardSurface;
        _boardLayerMask = boardLayerMask;
    }
    public bool TryGetBoardPosition(out Vector3 position) {
        position = Vector3.zero;
        var pointerPosition = Pointer.current?.position.ReadValue() ?? Vector2.zero;
        Ray ray = _mainCamera.ScreenPointToRay(pointerPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _boardLayerMask) && hit.collider.gameObject == _boardSurface) {
            position = hit.point;
            return true;
        }
        
        return false;
    }

    public bool IsPointerOverUI() {
        if (EventSystem.current == null) {
            Debug.LogWarning("EventSystem is not present in the scene.");
            return false;
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current) {
            position = Pointer.current?.position.ReadValue() ?? Vector2.zero
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        int transparentLayer = LayerMask.NameToLayer("RaycastHitTransparent");

        foreach (RaycastResult result in results) {
            if (result.gameObject.GetComponent<RectTransform>() == null) continue;
            if (result.gameObject.layer == transparentLayer) continue;
            return true;
        }

        return false;
    }
    public bool IsMouseOverGameWindow() {
        if (!Application.isFocused) {
            return false;
        }

        var pointerPosition = Pointer.current?.position.ReadValue() ?? Vector2.zero;

        return (pointerPosition.x >= 0 && pointerPosition.x <= Screen.width &&
                pointerPosition.y >= 0 && pointerPosition.y <= Screen.height);
    }
}
