using UnityEngine;

public class LandToken
{
    public LandId LandId { get; private set; }
    public PlayerColor PlayerColor { get; set; }
    public LandToken(LandId id) {
        LandId = id;
    }
}
