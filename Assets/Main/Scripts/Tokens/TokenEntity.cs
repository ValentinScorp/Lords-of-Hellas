public abstract class TokenEntity
{
    public TokenType Type { get; private set; }
    public RegionId RegionId { get; set; }   
    public LandId LandId { get; set; }

    protected TokenEntity(TokenType type) {
        Type = type;
    }
    public void PlaceOnBoard(RegionId regionId)
    {
        RegionId = regionId;
    }
}