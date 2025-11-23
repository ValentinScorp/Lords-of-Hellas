using UnityEngine;

public class RegionOwnerEvent : IGameEvent
{
    public RegionId regionId;
    public PlayerColor Color;

    public RegionOwnerEvent(RegionId regionId, PlayerColor color)
    {
        this.regionId = regionId;
        this.Color = color;
    }
}
