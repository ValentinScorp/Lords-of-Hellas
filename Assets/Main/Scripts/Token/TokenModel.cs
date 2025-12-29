using System;

public abstract class TokenModel
{
    public TokenType Type { get; private set; }
    public RegionId RegionId { get; set; }  

    public event Action<RegionId> OnRegionChanged; 
    
    protected TokenModel(TokenType type) {
        Type = type;
        RegionId = RegionId.Unknown;
    }
    public void MoveToRegion(RegionId regionId)
    {
        RegionId = regionId;
        OnRegionChanged?.Invoke(RegionId);
    }
}