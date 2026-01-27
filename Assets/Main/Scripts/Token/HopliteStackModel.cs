using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

[System.Serializable]
public class HopliteStackModel : TokenModel, IPlayerOwned
{
    private PlayerColor _playerColor;
    private List<HopliteModel> _hoplites = new List<HopliteModel>();
    public IReadOnlyList<HopliteModel> Hoplites => _hoplites;
    public int Count => _hoplites.Count;
    public PlayerColor PlayerColor => _playerColor;
    public event Action<int> OnCountChanged;

    public HopliteStackModel(PlayerColor color) : base(TokenType.HopliteStack)
    {
        _playerColor = color;
    }
    public HopliteStackModel(HopliteModel hoplite) : base(TokenType.HopliteStack)
    {
        _playerColor = hoplite.PlayerColor;
        AddHoplite(hoplite);
    }
    public HopliteStackModel(Player player) : base(TokenType.HopliteStack)
    {
        _playerColor = player.Color;
    }
    public void AddHoplite(HopliteModel hoplite)
    {
        _hoplites.Add(hoplite);
        hoplite.RegionId = RegionId;
        OnCountChanged?.Invoke(_hoplites.Count);
    }
    public bool RemoveHoplite(HopliteModel hopliteUnit)
    {
        if(!_hoplites.Remove(hopliteUnit)) {
            Debug.LogWarning("Hoplite not found in stack.");
            return false;
        } else {
            OnCountChanged?.Invoke(_hoplites.Count);
        }
        return true;
    }
    public HopliteModel RemoveHoplite()
    {
        if (_hoplites.Count == 0) {
            Debug.LogWarning("Trying to remove hoplite from an empty stack.");
            return null;
        }
        var index = _hoplites.Count - 1;
        var hoplite = _hoplites[index];
        _hoplites.RemoveAt(index);
        OnCountChanged?.Invoke(_hoplites.Count);
        return hoplite;
    }
    public bool TryTakeUnmovedHoplite(out HopliteModel hoplite)
    {
        foreach(var h in _hoplites) {
            if (!h.IsMoved()) {
                hoplite = h;
                return true;
            }
        }
        hoplite = null;
        return false;
    }
}
