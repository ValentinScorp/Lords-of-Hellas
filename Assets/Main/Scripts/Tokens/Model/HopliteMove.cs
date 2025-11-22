using UnityEngine;

public class HopliteMove
{
    public PlayerColor Color { get; private set; }
    public RegionId RegionOrigin { get; private set; }
    public RegionId RegionPlacement { get; private set; }
    public bool Moved => RegionOrigin != RegionPlacement; 
    public HopliteMove(PlayerColor color, RegionId origin) {
        Color = color;
        RegionOrigin = origin;
    }
    public void PlaceTo(RegionId regionId) {
        RegionPlacement = regionId;
    }
    public void ResetPlacement() {
        RegionPlacement = RegionOrigin;
    }
}
