using System;

internal class HoplitesMoveActionModel : RegularActionModel
{
    internal PlayerColor PlayerColor { get; private set; }
    internal int HoplitesMax { get; private set; }
    internal int HoplitesLeft {get; private set; }
    internal HoplitesMoveActionModel(Player player)
    {
        PlayerColor = player.Color;
        HoplitesLeft = HoplitesMax = player.Hero.Leadership;
        
        SetCanUndo(false);
    }
    internal bool CanMove()
    {
        return HoplitesLeft > 0;
    }
    internal void MakeStep()
    {
        if (CanMove()) {
            HoplitesLeft--;
        }
    }
    internal void ResetSteps()
    {
        HoplitesLeft = HoplitesLeft;
    }

}