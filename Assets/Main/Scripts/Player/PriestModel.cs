internal class PriestModel
{
    internal enum Placement
    {
        OffBoard,
        Pool,
        Monument
    }
    internal PlayerColor Color { get; }
    internal Placement PlacedAt { get; private set; }
    internal MonumentModel.GodType Monument { get; private set; }
    internal PriestModel(PlayerColor color)
    {
        Color = color;
    }
    internal bool TryMoveToPool()
    {
        if (PlacedAt != Placement.OffBoard) return false;

        PlacedAt = Placement.Pool;
        return true;
    }
    internal bool TryPray(MonumentModel monument)
    {
        if (monument is null) return false;
        if (PlacedAt != Placement.Pool) return false;
        if (!monument.TryPray(this)) return false;

        PlacedAt = Placement.Monument;
        Monument = monument.God;
        return true;
    }
}
