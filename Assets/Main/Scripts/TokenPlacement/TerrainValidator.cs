using UnityEngine;
using static Board;

public class TerrainValidator
{
    public enum GhostState
    {
        Neutral,
        Allowed,
        Forbidden
    }

    private LayerMask _forbiddenLayers = 0;
    private LayerMask _allowedLayers = 0;
    private float _checkRadius;
    private GhostState _currentState;

    public bool IsCurrentStateAllowed => _currentState == GhostState.Allowed;

    public TerrainValidator() {
        Init();
    }
    public void Init() {
        _allowedLayers = LayerUtils.GetMask("RegionArea");
        _forbiddenLayers = LayerUtils.GetMask("RegionBorder", "HoplonToken");
    }

    public void SetTokenRadius(float radius) {
        _checkRadius = radius * 0.3f; // TODO resolve scale
    }

    public GameObject GetPlacementObject(Vector3 position) {
        LayerMask combinedLayers = _allowedLayers | _forbiddenLayers;
        Collider[] colliders = Physics.OverlapSphere(position, _checkRadius, combinedLayers);

        foreach (var col in colliders) {
            if (col.CompareTag("PlaceableRegion")) {
                return col.gameObject;
            }
        }

        return null;
    }
    
    public GhostState ValidatePlacement(Vector3 position) {
        LayerMask combinedLayers = _allowedLayers | _forbiddenLayers;
        Collider[] colliders = Physics.OverlapSphere(position, _checkRadius, combinedLayers);

        foreach (var col in colliders) {
            if (ColliderCheckForbidden(col)) {
                _currentState = GhostState.Forbidden;
                return _currentState;
            }
        }

        foreach (var col in colliders) {
            if (ColliderCheckAllowed(col)) {
                _currentState = GhostState.Allowed;
                return _currentState;
            }
        }
        _currentState = GhostState.Neutral;
        return _currentState;
    }

    public GhostState GetGhostState() { 
        return _currentState; 
    }

    public bool TryGetRegionIdAtPosition(Vector3 position, out RegionId regionId) {
        regionId = RegionId.Unknown;

        var regionCollider = GetPlacementObject(position);
        if (regionCollider == null) {
            Debug.LogWarning("No region collider found at position!");
            return false;
        }

        var regionEffects = regionCollider.GetComponent<RegionAreaView>();
        if (regionEffects == null) {
            Debug.LogWarning($"No RegionEffectsManager on collider {regionCollider.name}!");
            return false;
        }

        regionId = regionEffects.Id;
        return true;
    }    

    private bool ColliderCheckForbidden(Collider col) => col.CompareTag("RegionBorder") || col.CompareTag("PlacedToken");
    private bool ColliderCheckAllowed(Collider col) => col.CompareTag("PlaceableRegion");
}
