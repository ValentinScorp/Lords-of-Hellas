[System.Serializable]
internal class HopliteModel : TokenModel
{
    private bool _isMoved;
    internal bool OnBoard { get; set; }

    internal HopliteModel(PlayerColor color) : base(TokenType.Hoplite, color)
    {
    }
    internal void SetOwner(PlayerColor color)
    {
        PlayerColor = color;
    }
    internal void MarkMoved()
    {
        OnBoard = true;
        _isMoved = true;
    }
    internal void ResetMove()
    {
        _isMoved = false;
    }
    internal bool IsMoved()
    {
        return _isMoved;
    }
}
