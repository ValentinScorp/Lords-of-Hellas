public abstract class TokenModel
{
    public TokenType Type { get; private set; }
    public RegionId RegionId { get; set; }   
    public LandId LandId { get; set; }
    public SpawnPoint SpawnPoint { get; set; }

    protected TokenModel(TokenType type) {
        Type = type;
        RegionId = RegionId.Unknown;
        LandId = LandId.Unknown;
        SpawnPoint = new();
    }
    public void PlaceOnBoard(RegionId regionId)
    {
        RegionId = regionId;
    }
}