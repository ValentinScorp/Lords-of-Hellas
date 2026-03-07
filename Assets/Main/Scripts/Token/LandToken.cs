using System;

internal class LandToken
{
    internal LandId LandId { get; private set; }
    internal PlayerColor OwnerColor { get; set; }

    internal LandToken(LandId id) {
        LandId = id;
        OwnerColor = PlayerColor.Grey;
    }
}
