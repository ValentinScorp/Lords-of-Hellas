
using UnityEngine;

public class SpawnPoint
{
    public RegionId RegionId { get; private set; }
    public Vector3 Position { get; private set; } 
    public bool IsOccupied { get; private set; }
    public SpawnPoint(RegionId regionId = RegionId.Unknown, Vector3 position = default) {
        RegionId = regionId;
        Position = position;
        IsOccupied = false;
    }
    public void Occupy() {
        IsOccupied = true;
    }
    public void Release() {
        IsOccupied = false;
    }
}
