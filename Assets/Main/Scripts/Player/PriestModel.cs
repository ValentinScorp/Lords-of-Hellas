using System;
internal class PriestModel
{
    internal enum Placement
    {
        OffBoard,
        InPool,
        OnBoard
    }
    internal PlayerColor Color { get; }
    internal Placement PlacedAt { get; private set; }
    internal MonumentModel.GodType Monument { get; private set; }

    internal event Action <PriestModel> PriestChangedStatus; 
    internal PriestModel(PlayerColor color)
    {
        Color = color;
    }
    internal bool TryMoveToPool()
    {
        if (PlacedAt != Placement.OffBoard) return false;

        PlacedAt = Placement.InPool;
        PriestChangedStatus?.Invoke(this);
        return true;
    }
    internal bool TryPray(MonumentModel monument)
    {
        if (monument is null) return false;
        if (PlacedAt != Placement.InPool) return false;
        if (!monument.TryPray(this)) return false;

        PlacedAt = Placement.OnBoard;
        Monument = monument.God;
        PriestChangedStatus?.Invoke(this);
        return true;
    }
}
