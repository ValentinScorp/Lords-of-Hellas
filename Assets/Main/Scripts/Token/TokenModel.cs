using System;
using UnityEngine;

internal abstract class TokenModel
{
    internal TokenType Type { get; private set; }
    internal RegionId RegionId { get; private set; }
    internal int NestId { get; private set; }
    internal PlayerColor PlayerColor { get; private protected set;}

    internal event Action<TokenModel> RegionChanged;

    protected TokenModel(TokenType type, PlayerColor color) {
        Type = type;
        PlayerColor = color;
        ClearNest();
    }
    // protected TokenModel(TokenType type, Player player) {
    //     Type = type;
    //     PlayerColor = player.Color;
    //     ClearNest();
    // }
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