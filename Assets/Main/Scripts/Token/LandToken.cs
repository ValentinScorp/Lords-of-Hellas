using UnityEngine;

internal class LandToken
{
    internal LandId LandId { get; private set; }
    internal PlayerColor PlayerColor { get; set; }
    internal LandToken(LandId id) {
        LandId = id;
    }
}
