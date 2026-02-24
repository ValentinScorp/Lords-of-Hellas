using System;
using UnityEngine;

public abstract class TokenModel
{
    public TokenType Type { get; private set; }
    public RegionId RegionId { get; private set; }
    public int NestId { get; private set; }
    public PlayerColor PlayerColor { get; protected set;}

    public event Action<TokenModel> RegionChanged;

    protected TokenModel(TokenType type, PlayerColor color) {
        Type = type;
        PlayerColor = color;
        ClearNest();
    }
    protected TokenModel(TokenType type, Player player) {
        Type = type;
        PlayerColor = player.Color;
        ClearNest();
    }
    internal bool IsOnBoard()
    {
        return RegionId != RegionId.Unknown;
    }
    internal void ClearNest()
    {
        NestId = -1;
    }
    
    internal void CopyBoardLocation(TokenModel other)
    {
        if (other == null) return;
        SetBoardLocation(other.RegionId, other.NestId);
    }
    internal void SetBoardLocation(RegionId regionId, int nestId)
    {
        RegionId = regionId;
        NestId = nestId;
        RegionChanged?.Invoke(this);
    }
}