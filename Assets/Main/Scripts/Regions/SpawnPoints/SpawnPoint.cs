
using UnityEngine;

public class SpawnPoint
{
    public RegionId RegionId { get; private set; }
    public Vector3 Position { get; private set; } 
    public bool IsOccupied { get; private set; }

    public SpawnPoint(RegionId regionId, int id, Vector3 position) {
        RegionId = regionId;
        Position = position;
    }
    public void Occupy() {
        IsOccupied = true;
    }
    public void Release() {
        IsOccupied = false;
    }
}
