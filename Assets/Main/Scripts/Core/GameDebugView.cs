using System.Collections.Generic;
using UnityEngine;

public class GameDebugView : MonoBehaviour
{
    [SerializeReference] private List<Player> _players;
    [SerializeReference] private GameState _gameState;
    private void Update()
    {
        _gameState = GameState.Instance;
       // _players = (List<Player>)_gameState.Players;
    }
}
