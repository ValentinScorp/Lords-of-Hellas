using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
public class SpawnPointGenerator : MonoBehaviour
{
    [SerializeField] private float _step = 1f;

    private Mesh _mesh;
    private List<SpawnPoint> _spawnPoints = new();

    public IReadOnlyList<SpawnPoint> SpawnPoints => _spawnPoints;

    private void Start() {
        _mesh = GetComponent<MeshFilter>().mesh;
        GenerateSpawnPoints();
    }
    public SpawnPoint GetFreeSpawnPoint() {
        foreach (var point in _spawnPoints) {
            if (!point.IsOccupied) {
                return point;
            }
        }
        Debug.LogError("No free SawnPoints left!");
        return null;
    }
    public SpawnPoint GetCenteredUnoccupied() {
        var average = CalcAveragePoint();
        return GetNearestUnoccupied(average);
    }
    public SpawnPoint GetNearestUnoccupied(Vector3 point) {
        SpawnPoint nearest = null;
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
    public void ReleaseSpawnPoint(int id) {
        foreach (var point in _spawnPoints) {
            if (point.Id == id) {
                point.Release();
                return;
            }
        }
        Debug.LogError("Couldn`t find SpawnPoint to Release! " + id.ToString());
    }
    private void GenerateSpawnPoints() {
        _spawnPoints.Clear();

        var vertices = _mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
        var bounds = _mesh.bounds;
        Vector3 min = transform.TransformPoint(bounds.min);
        Vector3 max = transform.TransformPoint(bounds.max);

        int idCounter = 0;
        for (float x = min.x; x <= max.x; x += _step) {
            for (float z = min.z; z <= max.z; z += _step) {
                Vector3 pos = new Vector3(x, 0f, z);
                if (IsPointInsideMesh(pos)) {
                    _spawnPoints.Add(new(idCounter++, pos));
                }
            }
        }

        //Debug.Log($"Generated {_spawnPoints.Count} spawn points for {name}");
    }
    private Vector3 CalcAveragePoint() {
        Vector3 averagePoint = Vector3.zero;
        foreach (var spawnPoint in _spawnPoints) {
            averagePoint += spawnPoint.Position;
        }
        averagePoint /= _spawnPoints.Count;
        return averagePoint;
    }
    private bool IsPointInsideMesh(Vector3 point) {
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

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        if (_spawnPoints == null) {
            return;
        }

        foreach (var p in _spawnPoints) {
            Gizmos.DrawSphere(p.Position, 0.1f);
        }
    }
}
