using System;
using UnityEngine;

public class HeroMoveAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private RegularAction _regularAction;
    private TokenView _heroToken;
    private MoveRoute _moveRoute;
    private Action _onComplete;
    public HeroMoveAction()
    {
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        _moveRoute = new MoveRoute();
    }
    public void Start(RegularAction regularAction, Action onComplete = null)
    {
        _regularAction = regularAction;
        _onComplete = onComplete;
        _tokenSelector.WaitTokenSelection(regularAction.Player.Color, TokenType.Hero, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        _heroToken = token;
        
        _moveRoute.SetSteps(_regularAction.HeroSteps); 
        _moveRoute.AddRouteNode(token.RegionId, token.SpawnPoint);

        _tokenMover.CreateGhostToken(token);
        _tokenMover.CatchNeibRegionPoint(token, token.RegionId, HandleStep);
    }
    private void HandleStep(TokenNest spawnPoint)
    {
        _regularAction.HeroSteps--;
        _moveRoute.AddRouteNode(spawnPoint.RegionId, spawnPoint);

        if (_moveRoute.Complete) {
            HandleMoveComplete(spawnPoint);
        } else {
            _tokenMover.CatchNeibRegionPoint(_heroToken, spawnPoint.RegionId, HandleStep);
        }
    }
    private void HandleMoveComplete(TokenNest spawnPoint)
    {
        ServiceLocator.Get<RegionsView>().PlaceTokenAtSpawn(_heroToken, spawnPoint);      
        ServiceLocator.Get<TokenVisualChanger>().PrepareTokenPlacement(_heroToken, _heroToken.PlayerColor);

        GameContext.Instance.RegionDataRegistry.RegisterToken(spawnPoint.RegionId, _heroToken.ViewModel.Model);

        _tokenMover.DestroyVisuals();
        _moveRoute.Clear();

        _onComplete?.Invoke();
    }
}
