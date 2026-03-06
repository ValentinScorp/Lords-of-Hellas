using UnityEngine;

internal class LandToken
{
    internal LandId LandId { get; private set; }
    internal PlayerColor Owner { get; private set; }

    internal LandToken(LandId id) {
        LandId = id;
        Owner = PlayerColor.Grey;
    }

    internal bool SetOwner(PlayerColor owner)
    {
        if (Owner == owner) {
            return false;
        }

        Owner = owner;
        return true;
    }
}
