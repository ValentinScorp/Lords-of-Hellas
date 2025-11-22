using UnityEngine;

public class LandToken : Token, IPlayerOwned
{
    public LandId LandId { get; private set; }
    public PlayerColor PlayerColor { get; set; }
    public LandToken(LandId id) : base(TokenType.Land) {
        LandId = id;
    }
}
