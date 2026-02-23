using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HopliteStackModel : TokenModel
{
    private PlayerColor _playerColor;
    private List<HopliteModel> _hoplites = new List<HopliteModel>();
    public IReadOnlyList<HopliteModel> Hoplites => _hoplites;
    public int Count => _hoplites.Count;
    public event Action<int> OnCountChanged;

    public HopliteStackModel(PlayerColor color) : base(TokenType.HopliteStack, color)
    {
        _playerColor = color;
    }
    public HopliteStackModel(Player player) : base(TokenType.HopliteStack, player)
    {
        _playerColor = player.Color;
    }
    public void AddHoplite(HopliteModel hoplite)
    {
        // Debug.Log($"Adding hoplite {RegionId}");
        hoplite.Nest = Nest;
        _hoplites.Add(hoplite);
        OnCountChanged?.Invoke(_hoplites.Count);
    }
    public bool RemoveHoplite(HopliteModel hoplite)
    {
        if(!_hoplites.Remove(hoplite)) {
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
