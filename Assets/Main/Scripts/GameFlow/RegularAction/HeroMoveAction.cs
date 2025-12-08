using UnityEngine;

public class HeroMoveAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private int _heroSpeed;
    private TokenView _heroToken;
    public HeroMoveAction()
    {
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
    }
    public void Start(Player player)
    {
        _heroSpeed = player.Hero.Speed;
        _tokenSelector.WaitTokenSelection(player.Color, TokenType.Hero, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        _heroToken = token;


        _tokenMover.StartMove(token, HandleStep);
    }
    private void HandleStep(RegionId regionId)
    {
        //HandleStep
    }
    private void HandleMoveComplete()
    {
        Debug.Log("Hero move complete!");
    }
}
