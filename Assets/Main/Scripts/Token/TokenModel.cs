using System;
using UnityEngine;

public abstract class TokenModel
{
    public TokenType Type { get; private set; }
    private RegionId _regionId;
    public event Action<TokenModel> RegionChanged;
    public RegionId RegionId { 
        get => _regionId; 
        set {
            if (_regionId == value) {
                return;
            }            
            Debug.Log($"TokenModel RegionId set: {_regionId} -> {value}");
            _regionId = value;
            RegionChanged?.Invoke(this);
        }
    }  
    public PlayerColor PlayerColor { get; protected set;}

    protected TokenModel(TokenType type, PlayerColor color) {
        Type = type;
        PlayerColor = color;
        RegionId = RegionId.Unknown;
    }
    protected TokenModel(TokenType type, Player player) {
        Type = type;
        PlayerColor = player.Color;
        RegionId = RegionId.Unknown;
    }
    internal bool IsOnBoard()
    {
        return _regionId != RegionId.Unknown;
    }
}