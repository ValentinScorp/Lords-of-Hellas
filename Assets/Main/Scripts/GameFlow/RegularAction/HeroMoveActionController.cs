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
        Debug.Log("Starting Hero Move Controller!");
        _onComplete = onComplete;
        _moveModel = moveModel;
        _tokenSelector.WaitTokenSelection(_moveModel.PlayerColor, TokenType.Hero, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        _heroToken = token;
        
        _moveRoute.SetSteps(_moveModel.StepsMax); 
        _moveRoute.AddRouteNode(token.RegionId, token.SpawnPoint);

        _tokenMover.CreateGhostToken(token);
        _tokenMover.CatchNeibRegionPoint(token, token.RegionId, HandleStep);
    }
    private void HandleStep(TokenNest spawnPoint)
    {
        _moveModel.MakeStep();
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

        GameContext.Instance.RegionDataRegistry.TryPlace(spawnPoint.RegionId, _heroToken.ViewModel.Model);

        _tokenMover.DestroyVisuals();
        _moveRoute.Clear();

        _onComplete?.Invoke();
    }
}
