using UnityEngine;

public class HopliteCountEvent : IGameEvent
{
    public RegionId RegionId { get; }
    public PlayerColor Color { get; }
    public int Count { get; }

    public HopliteCountEvent(RegionId regionId, PlayerColor color, int count) {
        RegionId = regionId;
        Color = color;
        Count = count;
    }
}
