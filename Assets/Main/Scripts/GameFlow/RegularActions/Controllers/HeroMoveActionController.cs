using System;
using UnityEngine;

public class HeroMoveActionController
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private readonly RegularActionConfirmPanel _actionControlPanel;
    private HeroMoveActionModel _moveModel;
    private TokenView _heroToken;
    private MoveRoute _moveRoute;
    private Action<RegularActionType> _onComplete;

    public HeroMoveActionController()
    {
        _actionControlPanel = SceneUiRegistry.Get<RegularActionConfirmPanel>();
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        _moveRoute = new MoveRoute();
    }
    public void Start(HeroMoveActionModel moveModel, Action<RegularActionType> onComplete = null)
    {
        _onComplete = onComplete;
        _moveModel = moveModel;
        
        if (_actionControlPanel is not null && _moveModel is not null) {
            _actionControlPanel.Show(true);
            _actionControlPanel.Bind(
                onDone: OnDone,
                onUndo: OnUndo,
                onCancel: OnCancel
            );
            _moveModel.CanUndoChanged += _actionControlPanel.SetUndoInteractable;
            _actionControlPanel.SetUndoInteractable(_moveModel.CanUndo);
        }
        _tokenSelector?.WaitTokenSelection(_moveModel.PlayerColor, TokenType.Hero, HandleSelection);
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

    public void OnDone()
    {        
        Cleanup();
        _onComplete?.Invoke(RegularActionType.HeroMove);
    }

    public void OnUndo()
    {
        _moveModel.UndoLast();
    }

    public void OnCancel()
    {
        Cleanup();
         _onComplete?.Invoke(RegularActionType.HeroMove);
    }
    private void Cleanup()
    {
        _moveModel.CanUndoChanged -= _actionControlPanel.SetUndoInteractable;

        _actionControlPanel.Unbind();
        _actionControlPanel.Show(false);
    }
}
