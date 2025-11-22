using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[System.Serializable]
public class HopliteStack : Token, IPlayerOwned
{
    [SerializeField] private PlayerColor _playerColor;
    [SerializeField] private int _count = 1;
    public int Count {
        get => _count;
        set {
            _count = value;
            OnCountChanged?.Invoke(_count);
        }
    }
    public PlayerColor PlayerColor => _playerColor;
    public event Action<int> OnCountChanged;
    public HopliteStack(Player player) :
        base (TokenType.Hoplite) {
        _playerColor = player.Color;
    }
}
