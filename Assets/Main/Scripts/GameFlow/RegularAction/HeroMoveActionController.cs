using System;
using UnityEngine;

public class HeroMoveActionController : IRegularAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private readonly ActionControlPanelController _actionControlPanelController;

    private HeroMoveActionModel _moveModel;
    private TokenView _heroToken;
    private MoveRoute _moveRoute;
    private Action _onComplete;

    public event Action Completed;

    public HeroMoveActionController()
    {
        _actionControlPanelController = ServiceLocator.Get<ActionControlPanelController>();
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        _moveRoute = new MoveRoute();
    }
    public void Start(HeroMoveActionModel moveModel, Action onComplete = null)
    {
        _onComplete = onComplete;
        _moveModel = moveModel;
        _actionControlPanelController.Start(this);
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
        Debug.Log("Move completed");
        var regionsRegistry = GameContext.Instance.RegionDataRegistry;
        
        regionsRegistry.Move(_heroToken.ViewModel.Model, nest);
        // regionsRegistry.TryTake(_heroToken.ViewModel.Model, _heroToken.RegionId);
        // regionsRegistry.TryPlace(_heroToken.ViewModel.Model, nest);

        _tokenMover.DestroyVisuals();
        _moveRoute.Clear();

        _onComplete?.Invoke();
    }

    public void Done()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }
}
