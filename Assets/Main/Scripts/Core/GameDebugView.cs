using System.Collections.Generic;
using UnityEngine;

public class GameDebugView : MonoBehaviour
{
    [SerializeReference] private List<Player> _players;
    [SerializeReference] private GameContext _gameState;
    private void Update()
    {
        _gameState = GameContext.Instance;
    }
}
