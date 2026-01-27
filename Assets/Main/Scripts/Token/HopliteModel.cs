[System.Serializable]
public class HopliteModel : TokenModel, IPlayerOwned
{
    private bool _isMoved;
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
    public bool IsMoved()
    {
        return _isMoved;
    }
}
