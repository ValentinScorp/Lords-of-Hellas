
using UnityEngine;

public class TokenNest
{
    public RegionId RegionId { get; private set; }
    public Vector3 Position { get; private set; } 
    public bool IsOccupied { get; private set; }
    public TokenNest(Vector3 position = default, RegionId regionId = RegionId.Unknown) {
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
