using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectsHitDetector
{
    private Camera _camera;
    private Action<List<Target>> _HitTargets;

    public class Target
    {
        public IHittable Hittable { get; private set; }
        public Vector3 HitPoint { get; private set; }

        public Target(IHittable selectable, Vector3 hitPoint)
        {
            Hittable = selectable;
            HitPoint = hitPoint;
        }
    }
    public ObjectsHitDetector(Camera camera, UserInputController userInputController)
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
    public void HandleHits(Vector2 screenPosition)
    {
        // Debug.Log($"ClickMgr: HandleClick at {screenPosition}");
        if (IsMouseOverCanvas(ServiceLocator.Get<Canvas>(), screenPosition)) return;
        // Debug.Log("ClickMgr: Click not over canvas, proceeding with raycast.");

        var ray = _camera.ScreenPointToRay((Vector3)screenPosition);
        var hits = Physics.RaycastAll(ray, _camera.farClipPlane);

        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        List<Target> hitTargets = new List<Target>();

        foreach (var hit in hits) {
            //  Debug.Log($"ClickMgr: Raycast hit {hit.collider.gameObject.name}");
            foreach (var selectable in hit.collider.GetComponents<IHittable>()) {
                hitTargets.Add(new Target(selectable, hit.point));
            }
        }
        if (_HitTargets is not null) {
            if (hitTargets.Count > 0) {
                _HitTargets.Invoke(hitTargets);
            }
        } 
        // else {
        //     foreach (var ct in hitTargets) {
        //         if (ct.Selectable is RegionAreaView regionArea) {
        //             _regionInfoController?.Select(regionArea);
        //             break;
        //         }
        //     }
        // }
    }
    private bool IsMouseOverCanvas(Canvas canvas, Vector2 screenPosition)
    {
        if (canvas == null || EventSystem.current == null) 
            return false;

        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        
        if (gr == null) 
            return false;

        var ped = new PointerEventData(EventSystem.current) {
            position = screenPosition
        };
        var results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        // foreach (var result in results)
        //     Debug.Log($"MouseOverCanvas hit: {result.gameObject.name}");

        return results.Count > 0;
    }
    public void Listen(Action<List<Target>> HitTargets)
    {
        if (_HitTargets != null) {
            Debug.LogWarning("ClickMgr: Overwriting existing click listener!");
        }
        // Debug.Log("ClickMgr: Listening for clicks.");
        _HitTargets = HitTargets;
        var regionInfoCtlr = ServiceLocator.Get<RegionInfoUiCtlr>();
        regionInfoCtlr.Deactivate();
    }
    public void Unlisten()
    {
        _HitTargets = null;
    }
}


