using System;

public abstract class TokenModel
{
    public TokenType Type { get; private set; }
    public RegionId RegionId { get; set; }  
    public PlayerColor PlayerColor { get; protected set;}

    public event Action<RegionId> OnRegionChanged; 
    protected TokenModel(TokenType type, PlayerColor color) {
        Type = type;
        PlayerColor = color;
        RegionId = RegionId.Unknown;
    }
    protected TokenModel(TokenType type, Player player) {
        Type = type;
        PlayerColor = player.Color;
        RegionId = RegionId.Unknown;
    }
    public void MoveToRegion(RegionId regionId)
    {
        RegionId = regionId;
        OnRegionChanged?.Invoke(RegionId);
    }
}