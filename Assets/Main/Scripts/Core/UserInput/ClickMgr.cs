using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClickMgr
{
    private Camera _camera;    
    // private EventSystem _eventSystem;
    // private UserInputController _userInputController;
    // private Action<TokenView> _onTokenClicked;
    // private Action<RegionId> _onRegionClicked;
    private Action<List<IClickable>> _onClicked;

    public ClickMgr(Camera camera, UserInputController userInputController)
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
        if (IsMouseOverCanvas(ServiceLocator.Get<Canvas>(), screenPosition)) return;

        var ray = _camera.ScreenPointToRay((Vector3)screenPosition);
        var hits = Physics.RaycastAll(ray, _camera.farClipPlane);
        
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        List<IClickable> clickables = new List<IClickable>();
        
        foreach (var hit in hits) {
            Debug.Log($"ClickMgr: Raycast hit {hit.collider.gameObject.name}");
            clickables.AddRange(hit.collider.GetComponents<IClickable>());
        }
        if (clickables.Count > 0)
            _onClicked?.Invoke(clickables);        
    }

    public void ListenClicks(Action<List<IClickable>> onClicked)
    {
        if (_onClicked != null)        {
            Debug.LogWarning("ClickMgr: Overwriting existing click listener!");
        }
        _onClicked = onClicked;
    }
    public void UnlistenClicks()
    {
        _onClicked = null;
    }
}

public interface IClickable
{
    
}

