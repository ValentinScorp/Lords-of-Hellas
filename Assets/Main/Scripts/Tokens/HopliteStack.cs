using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HopliteStack : TokenEntity, IPlayerOwned
{
    private PlayerColor _playerColor;
    private List<HopliteUnit> _hoplites = new List<HopliteUnit>();
    public int Count => _hoplites.Count;
    public PlayerColor PlayerColor => _playerColor;
    public event Action<int> OnCountChanged;

    public HopliteStack(PlayerColor color) : base(TokenType.HopliteStack)
    {
        _playerColor = color;
    }
    public HopliteStack(Player player) : base(TokenType.HopliteStack)
    {
        _playerColor = player.Color;
    }
    public void AddHoplite(HopliteUnit hoplite)
    {
        _hoplites.Add(hoplite);
        EventBus.SendEvent(new HopliteCountEvent(RegionId, _playerColor, _hoplites.Count));
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
        EventBus.SendEvent(new HopliteCountEvent(RegionId, _playerColor, _hoplites.Count));
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
