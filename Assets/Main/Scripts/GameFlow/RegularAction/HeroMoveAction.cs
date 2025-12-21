using UnityEngine;

public class HeroMoveAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private int _stepsLeft;
    private TokenView _heroToken;
    public HeroMoveAction()
    {
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
    }
    public void Start(Player player)
    {
        _stepsLeft = player.Hero.Speed;
        _tokenSelector.WaitTokenSelection(player.Color, TokenType.Hero, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        _heroToken = token;
        _tokenMover.StartMove(token, token.Model.RegionId, HandleStep);
    }
    private void HandleStep(RegionId regionId, int spawnPoint)
    {        
        _stepsLeft--;
        Debug.Log($"Steps left : {_stepsLeft}");
        if (_stepsLeft <= 0) {
            
            HandleMoveComplete();
        } else {
            _tokenMover.StartMove(_heroToken, regionId, HandleStep);
        }
    }
    private void HandleMoveComplete()
    {
        Debug.Log("Hero move complete!");
    }
}
