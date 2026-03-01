using System;

internal class HeroMoveActionModel : RegularActionModel
{
    internal PlayerColor PlayerColor { get; }
    internal int StepsMax { get; }
    internal int StepsLeft { get; private set; }

    internal HeroMoveActionModel(Player player)
    {
        PlayerColor = player.Color;
        StepsMax = StepsLeft = player.Hero.Speed;

        SetCanUndo(false);
    }

    internal bool CanMove() => StepsLeft > 0;

    internal void MakeStep()
    {
        if (CanMove()) {
            StepsLeft--;
            SetCanUndo(true);
        }
    }
}
