using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SelectMgr
{
    private Camera _camera;    
    // private EventSystem _eventSystem;
    // private UserInputController _userInputController;
    // private Action<TokenView> _onTokenClicked;
    // private Action<RegionId> _onRegionClicked;
    private RegionInfoUiController _regionInfoUiController;
    private Action<List<ISelectable>> _onSelected;

    public SelectMgr(Camera camera, UserInputController userInputController)
    {
        if (camera == null) {
            Debug.LogWarning("Error in input parameter Camera!");
            return;
        } else {
            _camera = camera;
        }        
        
        if (userInputController == null) {
            Debug.LogWarning("Error in input parameter UserInputController!");
        } else {
            userInputController.SetClickMgr(this);
        }        
    }
    public bool IsMouseOverCanvas(Canvas canvas, Vector2 screenPosition)
    {
        if (canvas == null || EventSystem.current == null) return false;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        if (gr == null) return false;

        var ped = new PointerEventData(EventSystem.current) { 
            position = screenPosition 
        };
        var results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        foreach (var result in results)
            Debug.Log($"MouseOverCanvas hit: {result.gameObject.name}");

        return results.Count > 0;
    }

    public void HandleClick(Vector2 screenPosition)
    {
        Debug.Log($"ClickMgr: HandleClick at {screenPosition}");
        if (IsMouseOverCanvas(ServiceLocator.Get<Canvas>(), screenPosition)) return;
        Debug.Log("ClickMgr: Click not over canvas, proceeding with raycast.");

        var ray = _camera.ScreenPointToRay((Vector3)screenPosition);
        var hits = Physics.RaycastAll(ray, _camera.farClipPlane);
        
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        List<ISelectable> selectables = new List<ISelectable>();
        
        foreach (var hit in hits) {
            Debug.Log($"ClickMgr: Raycast hit {hit.collider.gameObject.name}");
            
            foreach (var selectable in hit.collider.GetComponents<ISelectable>()) {
                selectable.HitPoint = hit.point;
                selectables.Add(selectable);
            }            
        }
        if (selectables.Count > 0)
            _onSelected?.Invoke(selectables);

        foreach (var sel in selectables) {
            if (sel is RegionAreaView regionArea) {
                _regionInfoUiController?.Select(regionArea);
                break;
            }
        }    
    }

    public void ListenSelection(Action<List<ISelectable>> onSelected)
    {
        if (_onSelected != null) {
            Debug.LogWarning("ClickMgr: Overwriting existing click listener!");
        }
        Debug.Log("ClickMgr: Listening for clicks.");
        _onSelected = onSelected;
    }
    public void UnlistenSelection()
    {
        _onSelected = null;
    }
    public void RegisterSelctionListener(RegionInfoUiController regionInfoUiController)
    {
        _regionInfoUiController = regionInfoUiController;
    }
}


