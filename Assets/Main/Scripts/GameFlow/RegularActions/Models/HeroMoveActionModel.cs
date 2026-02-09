using System;

public class HeroMoveActionModel : RegularActionModel
{
    public PlayerColor PlayerColor { get; }
    public int StepsMax { get; }
    public int StepsLeft { get; private set; }

    public HeroMoveActionModel(Player player)
    {
        PlayerColor = player.Color;
        StepsMax = StepsLeft = player.Hero.Speed;

        SetCanUndo(false);
    }

    public bool CanMove() => StepsLeft > 0;

    public void MakeStep()
    {
        if (CanMove()) {
            StepsLeft--;
            SetCanUndo(true);
        }
    }
}
