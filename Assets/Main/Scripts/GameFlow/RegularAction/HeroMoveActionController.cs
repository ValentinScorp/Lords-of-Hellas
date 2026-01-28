using System;
using UnityEngine;

public class HeroMoveActionController
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private HeroMoveActionModel _moveModel;
    private TokenView _heroToken;
    private MoveRoute _moveRoute;
    private Action _onComplete;
    public HeroMoveActionController()
    {
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        _moveRoute = new MoveRoute();
    }
    public void Start(HeroMoveActionModel moveModel, Action onComplete = null)
    {
        _onComplete = onComplete;
        _moveModel = moveModel;
        Debug.Log($"{_moveModel.PlayerColor}");
        _tokenSelector.WaitTokenSelection(_moveModel.PlayerColor, TokenType.Hero, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        _heroToken = token;
        
        _moveRoute.AddRouteNode(token.RegionId, token.Nest);

        _tokenMover.Init(token);
        _tokenMover.CatchNeibRegionPoint(token.RegionId, HandleStep);
    }
    private void HandleStep(TokenNest nest)
    {
        _moveModel.MakeStep();
        _moveRoute.AddRouteNode(nest.RegionId, nest);

        if (!_moveModel.CanMove()) {
            HandleMoveComplete(nest);
        } else {
            _tokenMover.CatchNeibRegionPoint(nest.RegionId, HandleStep);
        }
    }
    private void HandleMoveComplete(TokenNest nest)
    {
        var regionsRegistry = GameContext.Instance.RegionDataRegistry;
        
        regionsRegistry.TryTake(_heroToken.ViewModel.Model, _heroToken.RegionId);
        regionsRegistry.TryPlace(_heroToken.ViewModel.Model, nest);

        _tokenMover.DestroyVisuals();
        _moveRoute.Clear();

        _onComplete?.Invoke();
    }


}
