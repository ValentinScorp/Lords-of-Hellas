
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class SpawnPoint
{
    public int Id { get; private set; }
    public Vector3 Position { get; private set; } 
    public bool IsOccupied { get; private set; }

    public SpawnPoint(int id, Vector3 position) {
        Id = id;
        Position = position;
    }
    public void Occupy() {
        IsOccupied = true;
    }
    public void Release() {
        IsOccupied = false;
    }
}
