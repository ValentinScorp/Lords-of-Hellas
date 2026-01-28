[System.Serializable]
public class HopliteModel : TokenModel
{
    private bool _isMoved;
    public bool OnBoard { get; set; }

    public HopliteModel(Player player) : base(TokenType.Hoplite, player)
    {
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
