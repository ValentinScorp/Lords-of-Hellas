using UnityEngine;

[System.Serializable]
public class HopliteUnit : IPlayerOwned
{
    private bool _moved;
    private RegionId _regionId;

    public int Id { get; private set; }
    public bool OnBoard { get; set; }
    public RegionId RegionId => _regionId;
    public PlayerColor PlayerColor { get; private set; }

    public HopliteUnit(PlayerColor color, int id)
    {
        PlayerColor = color;
        Id = id;
    }

    public HopliteUnit(int id)
    {
        Id = id;
    }
    public void SetOwner(PlayerColor color)
    {
        PlayerColor = color;
    }
    public void ChangeRegion(RegionId regionId)
    {
        _regionId = regionId;
        OnBoard = true;
        _moved = false;
    }
}
