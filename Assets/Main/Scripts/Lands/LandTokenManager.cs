using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandTokenManager
{
    private List<LandToken> _tokens = new();
    public LandTokenManager() {        
        foreach (LandId id in Enum.GetValues(typeof(LandId))) {
            _tokens.Add(new LandToken(id));
        }
    }
    public void Subscribe(Player player) {
        player.OnAddLandToken += SendTokenToPlayer;
    }
    public void SendTokenToPlayer(Player player, LandId landId) {
        var token = GetToken(landId);
        if (token == null) {
            Debug.LogError($"Token with LandId '{landId}' not found!");
            return;
        }

        if (token.PlayerColor != PlayerColor.Gray) {
            Debug.Log($"Token '{landId}' already belongs to {token.PlayerColor}!");
        }
        token.PlayerColor = player.Color;

        Debug.Log($"Token '{landId}' sent to player {player.Color}.");
    }
    public void ReturnToken(LandId id) {
        var token = GetToken(id);
        if (token != null) { 
            token.PlayerColor = PlayerColor.Gray;
        }
    }
    private LandToken GetToken(LandId id) => _tokens.FirstOrDefault(t => t.LandId == id);
}
