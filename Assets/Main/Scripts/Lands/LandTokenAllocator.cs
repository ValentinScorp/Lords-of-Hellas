using System;
using System.Collections.Generic;

internal class LandTokenAllocator
{
    private readonly Dictionary<LandId, LandToken> _tokens = new();
    private readonly Dictionary<PlayerColor, Player> _players = new();
   
    internal LandTokenAllocator(List<Player> players)
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
}
