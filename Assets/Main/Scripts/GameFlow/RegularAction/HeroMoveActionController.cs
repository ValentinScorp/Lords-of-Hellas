using System;
using UnityEngine;

public class HeroMoveActionController : IRegularAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private readonly ActionControlPanel _actionControlPanel;
    private HeroMoveActionModel _moveModel;
    private TokenView _heroToken;
    private MoveRoute _moveRoute;
    private Action _onComplete;


    public HeroMoveActionController()
    {
        _actionControlPanel = SceneUIRegistry.Get<ActionControlPanel>();
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        _moveRoute = new MoveRoute();
    }
    public void Start(HeroMoveActionModel moveModel, Action onComplete = null)
    {
        _onComplete = onComplete;
        _moveModel = moveModel;
        
        _actionControlPanel?.Show(true);
        _actionControlPanel?.Bind(
            onDone: Done,
            onUndo: Undo,
            onCancel: Cancel
        );
        _moveModel.CanUndoChanged += _actionControlPanel.SetUndoInteractable;
        _actionControlPanel.SetUndoInteractable(_moveModel.CanUndo);

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
        Debug.Log("Hero Move completed");
        var regionsRegistry = GameContext.Instance.RegionDataRegistry;        
        regionsRegistry.Move(_heroToken.ViewModel.Model, nest);

        _tokenMover.DestroyVisuals();
        _moveRoute.Clear(); 
    }

    public void Done()
    {        
        Cleanup();
        _onComplete?.Invoke();
    }

    public void Undo()
    {
        _moveModel.UndoLast();
    }

    public void Cancel()
    {
        Cleanup();
    }
     private void Cleanup()
    {
        _moveModel.CanUndoChanged -= _actionControlPanel.SetUndoInteractable;

        _actionControlPanel.Unbind();
        _actionControlPanel.Show(false);
    }
}
