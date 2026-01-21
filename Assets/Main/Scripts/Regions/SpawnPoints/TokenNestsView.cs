using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TokenNestsView : MonoBehaviour
{
    [SerializeField] private float _step = 1f;
    private RegionId _regionId;
    private List<TokenNest> _spawnPoints = new();

    private void Start()
    {
         _regionId = GetComponent<RegionAreaView>().Id;
        GenerateNests();

        if (ServiceLocator.Get<RegionsViewModel>().TryGetRegion(_regionId, out var region)) {
            region.SetNests(_spawnPoints);
        }
    }
    public TokenNest GetFreeNest()
    {
        foreach (var point in _spawnPoints) {
            if (!point.IsOccupied) {
                return point;
            }
        }
        Debug.LogError("No free SawnPoints left!");
        return null;
    }
    public TokenNest GetCenteredUnoccupied()
    {
        var average = CalcAverageNest();
        return GetNearestUnoccupied(average);
    }
    public TokenNest GetNearestUnoccupied(Vector3 point)
    {
        TokenNest nearest = null;
        float minDistance = float.MaxValue;

        foreach (var spawnPoint in _spawnPoints) {
            if (!spawnPoint.IsOccupied) {
                float distance = Vector3.Distance(point, spawnPoint.Position);
                if (distance < minDistance) {
                    minDistance = distance;
                    nearest = spawnPoint;
                }
            }
        }
        if (nearest == null) {
            Debug.LogError("No free SpawnPoints left!");
        }
        return nearest;
    }
    private void GenerateNests()
    {
        _spawnPoints.Clear();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        var vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
        var bounds = mesh.bounds;
        Vector3 min = transform.TransformPoint(bounds.min);
        Vector3 max = transform.TransformPoint(bounds.max);

        for (float x = min.x; x <= max.x; x += _step) {
            for (float z = min.z; z <= max.z; z += _step) {
                Vector3 pos = new Vector3(x, 0f, z);
                if (IsPointInsideMesh(pos)) {
                    _spawnPoints.Add(new(pos, _regionId));
                }
            }
        }

        //Debug.Log($"Generated {_spawnPoints.Count} spawn points for {name}");
    }
    private Vector3 CalcAverageNest()
    {
        Vector3 averagePoint = Vector3.zero;
        foreach (var spawnPoint in _spawnPoints) {
            averagePoint += spawnPoint.Position;
        }
        averagePoint /= _spawnPoints.Count;
        return averagePoint;
    }
    private bool IsPointInsideMesh(Vector3 point)
    {
        int hits = 0;
        var rayUp = new Ray(point + Vector3.down * 10f, Vector3.up);
        if (Physics.Raycast(rayUp, out RaycastHit hitUp, 50f)) {
            if (hitUp.transform == transform) {
                hits++;
            }
        }
        var rayDown = new Ray(point + Vector3.up * 10f, Vector3.down);
        if (Physics.Raycast(rayDown, out RaycastHit hitDown, 50f)) {
            if (hitDown.transform == transform) {
                hits++;
            }
        }
        return hits % 2 == 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (_spawnPoints == null) {
            return;
        }
        foreach (var p in _spawnPoints) {
            Gizmos.DrawSphere(p.Position, 0.1f);
        }
    }
}
