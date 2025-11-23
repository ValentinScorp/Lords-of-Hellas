using System.Collections.Generic;
using System.Linq;

public class PlayerDataManager
{
    private List<Player> _players;
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    public PlayerDataManager(IEnumerable<Player> players)
    {
        _players = players.ToList();
    }
}

