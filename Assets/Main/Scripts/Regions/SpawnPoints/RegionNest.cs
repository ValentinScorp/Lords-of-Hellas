
using UnityEngine;

public class RegionNest
{
    public RegionId RegionId { get; private set; }
    public int Id { get; private set; }
    public Vector3 Position { get; private set; } 
    public bool IsOccupied { get; private set; }
    public RegionNest() : this(default, RegionId.Unknown, -1)
    {        
    }
    public RegionNest(Vector3 position, RegionId regionId, int id) {
        RegionId = regionId;
        Id = id;
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
