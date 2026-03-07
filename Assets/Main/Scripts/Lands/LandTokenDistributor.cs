using System;
using System.Collections.Generic;

internal class LandTokenDistributor
{
    private readonly Dictionary<LandId, LandToken> _tokens = new();
    private readonly Dictionary<PlayerColor, Player> _players = new();

    internal event Action<LandToken, PlayerColor> TokenOwnerChanged;
    internal LandTokenDistributor(List<Player> players)
    {
        foreach (LandId id in Enum.GetValues(typeof(LandId))) {
            if (id == LandId.Unknown) {
                continue;
            }
            _tokens[id] = new LandToken(id);
        }
        foreach (var p in players) {
            _players[p.Color] = p;
        }
    }

    private bool TryTransferToken(PlayerColor newOwner, RegionId regionId)
    {
        var landId = GameContext.Instance.RegionRegistry.GetLandId(regionId);
        if (landId == LandId.Unknown || !_tokens.TryGetValue(landId, out var token)) {
            return false;
        }

        if (newOwner == PlayerColor.Grey || !_players.TryGetValue(newOwner, out var newOwnerPlayer)) {
            return false;
        }

        var currentOwner = token.OwnerColor;
        if (currentOwner == newOwner) {
            return true;
        }

        if (currentOwner != PlayerColor.Grey && _players.TryGetValue(currentOwner, out var oldOwnerPlayer)) {
            oldOwnerPlayer.RemoveToken(landId);
        }

        if (!newOwnerPlayer.ReceiveToken(landId)) {
            return false;
        }

        token.OwnerColor = newOwner;
        TokenOwnerChanged?.Invoke(token, currentOwner);
        return true;
    }
    internal void TransferToken(PlayerColor color, RegionId regionId)
    {
        if (!TryTransferToken(color, regionId)) {
            UnityEngine.Debug.LogWarning($"TransferToken failed for owner {color} and region {regionId}.");
        }
    }
}
