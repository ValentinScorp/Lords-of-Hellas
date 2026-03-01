using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
internal class HopliteStackModel : TokenModel
{
    private PlayerColor _playerColor;
    private List<HopliteModel> _hoplites = new List<HopliteModel>();
    internal IReadOnlyList<HopliteModel> Hoplites => _hoplites;
    internal int Count => _hoplites.Count;
    internal event Action<int> CountChanged;

    internal HopliteStackModel(PlayerColor color) : base(TokenType.HopliteStack, color)
    {
        _playerColor = color;
    }
    internal void AddHoplite(HopliteModel hoplite)
    {
        // Debug.Log($"Adding hoplite {RegionId}");
        hoplite.CopyBoardLocation(this);

        _hoplites.Add(hoplite);
        CountChanged?.Invoke(_hoplites.Count);
    }
    internal bool RemoveHoplite(HopliteModel hoplite)
    {
        if(!_hoplites.Remove(hoplite)) {
            Debug.LogWarning("Hoplite not found in stack.");
            return false;
        } else {
            CountChanged?.Invoke(_hoplites.Count);
        }
        return true;
    }
    internal HopliteModel RemoveHoplite()
    {
        if (_hoplites.Count == 0) {
            Debug.LogWarning("Trying to remove hoplite from an empty stack.");
            return null;
        }
        var index = _hoplites.Count - 1;
        var hoplite = _hoplites[index];
        _hoplites.RemoveAt(index);
        CountChanged?.Invoke(_hoplites.Count);
        return hoplite;
    }
    internal bool TryTakeUnmovedHoplite(out HopliteModel hoplite)
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

    internal void RefreshCount()
    {
        CountChanged?.Invoke(_hoplites.Count);
    }
}
