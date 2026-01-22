[System.Serializable]
public class HopliteModel : TokenModel, IPlayerOwned
{
    public bool _isMoved { get; private set; }
    public bool OnBoard { get; set; }
    public PlayerColor PlayerColor { get; private set; }

    public HopliteModel(PlayerColor color) : base(TokenType.Hoplite)
    {
        PlayerColor = color;
    }
    public void SetOwner(PlayerColor color)
    {
        PlayerColor = color;
    }
    public void MarkModved()
    {
        OnBoard = true;
        _isMoved = true;
    }
    public void ResetMove()
    {
        _isMoved = false;
    }
}
