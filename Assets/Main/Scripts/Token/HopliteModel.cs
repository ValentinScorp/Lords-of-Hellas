using UnityEngine;

[System.Serializable]
public class HopliteModel : IPlayerOwned
{
    public bool Moved { get; private set; }
    private RegionId _regionId;

    public bool OnBoard { get; set; }
    public RegionId RegionId => _regionId;
    public PlayerColor PlayerColor { get; private set; }

    public HopliteModel(PlayerColor color)
    {
        PlayerColor = color;
    }

    public void SetOwner(PlayerColor color)
    {
        PlayerColor = color;
    }
    public void ChangeRegion(RegionId regionId)
    {
        _regionId = regionId;
        OnBoard = true;
        Moved = true;
    }
    public void ResetMove()
    {
        Moved = false;
    }
}
