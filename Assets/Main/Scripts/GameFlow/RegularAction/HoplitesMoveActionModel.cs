    public class HoplitesMoveActionModel
    {
        public PlayerColor PlayerColor { get; private set; }
        public int HoplitesMax { get; private set; }
        public int HoplitesLeft {get; private set; }
        public HoplitesMoveActionModel(Player player)
        {
            PlayerColor = player.Color;
            HoplitesLeft = HoplitesMax = player.Hero.Leadership;
        }
        public bool CanMove()
        {
            return HoplitesLeft > 0;
        }
        public void MakeStep()
        {
            if (CanMove()) {
                HoplitesLeft--;
            }
        }
        public void ResetSteps()
        {
            HoplitesLeft = HoplitesLeft;
        }
    }