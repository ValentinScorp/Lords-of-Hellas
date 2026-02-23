using System;
using UnityEngine;

public abstract class TokenModel
{
    public TokenType Type { get; private set; }
    public RegionNest Nest { get; set; }
    public event Action<TokenModel> RegionChanged;
    public RegionId RegionId { 
        get => Nest.RegionId;         
    }
    public PlayerColor PlayerColor { get; protected set;}

    protected TokenModel(TokenType type, PlayerColor color) {
        Type = type;
        PlayerColor = color;
        Nest = new();
    }
    protected TokenModel(TokenType type, Player player) {
        Type = type;
        PlayerColor = player.Color;
        Nest = new();
    }
    internal bool IsOnBoard()
    {
        return RegionId != RegionId.Unknown;
    }
    internal void ClearNest()
    {
        Nest = new();
    }
}