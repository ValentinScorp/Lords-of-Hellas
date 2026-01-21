using System.Collections.Generic;
using UnityEngine;

public class RegionViewModel
{
    public RegionId Id { get; }
    private List<SpawnPoint> _spawnPoints;

    public RegionViewModel(RegionId id)
    {
        Id = id;
    }
    public void SetSpawnPoints(List<SpawnPoint> spawnPoints)
    {
        _spawnPoints = spawnPoints;
    }
    public bool TryGetFreeSpawnPoint(Vector3 hitPoint, out SpawnPoint spawnPoint)
    {
        spawnPoint = GetNearestUnoccupied(hitPoint);
        if (spawnPoint != null) {
            return true;
        }
        return false;
    }
    public SpawnPoint GetCenteredUnoccupied()
    {
        var average = CalcAveragePoint();
        return GetNearestUnoccupied(average);
    }    
    public SpawnPoint GetNearestUnoccupied(Vector3 point)
    {
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
    private Vector3 CalcAveragePoint()
    {
        Vector3 averagePoint = Vector3.zero;
        foreach (var spawnPoint in _spawnPoints) {
            averagePoint += spawnPoint.Position;
        }
        averagePoint /= _spawnPoints.Count;
        return averagePoint;
    }
}
