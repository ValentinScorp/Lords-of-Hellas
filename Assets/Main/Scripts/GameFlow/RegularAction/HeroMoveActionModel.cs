public class HeroMoveActionModel
{
    public PlayerColor PlayerColor { get; private set; }
    public int StepsMax { get; private set; }
    public int StepsLeft {get; private set; }
    public HeroMoveActionModel(Player player)
    {
        StepsMax = StepsLeft = player.Hero.Speed;
    }
    public bool CanMove()
    {
        return StepsLeft > 0;
    }
    public void MakeStep()
    {
        if (CanMove()) {
            StepsLeft--;
        }
    }
    public void ResetSteps()
    {
        StepsLeft = StepsMax;
    }
}
