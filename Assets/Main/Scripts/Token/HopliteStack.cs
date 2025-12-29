using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HopliteStack : TokenModel, IPlayerOwned
{
    private PlayerColor _playerColor;
    private List<HopliteUnit> _hoplites = new List<HopliteUnit>();
    public IReadOnlyList<HopliteUnit> Hoplites => _hoplites;
    public int Count => _hoplites.Count;
    public PlayerColor PlayerColor => _playerColor;
    public event Action<int> OnCountChanged;

    public HopliteStack(PlayerColor color) : base(TokenType.HopliteStack)
    {
        _playerColor = color;
    }
    public HopliteStack(HopliteUnit hopliteUnit) : base(TokenType.HopliteStack)
    {
        _playerColor = hopliteUnit.PlayerColor;
        AddHoplite(hopliteUnit);
    }
    public HopliteStack(Player player) : base(TokenType.HopliteStack)
    {
        _playerColor = player.Color;
    }
    public void AddHoplite(HopliteUnit hoplite)
    {
        _hoplites.Add(hoplite);
        OnCountChanged?.Invoke(_hoplites.Count);
    }
    public bool RemoveHoplite(HopliteUnit hopliteUnit)
    {
        if(!_hoplites.Remove(hopliteUnit)) {
            Debug.LogWarning("Hoplite not found in stack.");
            return false;
        } else {
            OnCountChanged?.Invoke(_hoplites.Count);
        }
        return true;
    }
    public HopliteUnit RemoveHoplite()
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
    public void ChangeHoplitesRegion(RegionId regionId)
    {
        foreach(var h in _hoplites)
        {
            h.ChangeRegion(regionId);
        }
    }
}
