using System;
using UnityEngine;

public class HoplitesMoveActionController
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private HoplitesMoveActionModel _moveModel;
    private HopliteModel _hopliteModel;

    private RegularActionConfirmPanel _actionControlPanel;
    private Action<RegularActionType> _onComplete;

    public HoplitesMoveActionController()
    {
        _actionControlPanel = SceneUiRegistry.Get<RegularActionConfirmPanel>();
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        if (_tokenSelector is null || _tokenMover is null) {
            Debug.LogError("HoplitesMoveActionController constructor error!");
        }
    }
    public void Start(HoplitesMoveActionModel moveModel, Action<RegularActionType> onComplete = null)
    {
        _onComplete = onComplete;
        _moveModel = moveModel;

        _actionControlPanel?.Show(true);
        _actionControlPanel?.Bind(
            onDone: OnDone,
            onUndo: OnUndo,
            onCancel: OnCancel
        );
        _moveModel.CanUndoChanged += _actionControlPanel.SetUndoInteractable;
        _actionControlPanel.SetUndoInteractable(_moveModel.CanUndo);

        _tokenSelector.WaitTokenSelection(_moveModel.PlayerColor, TokenType.HopliteStack, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        if (token.ViewModel.Model is HopliteStackModel hsm) {
            if (hsm.TryTakeUnmovedHoplite(out var unmovedHoplite)) {
                _hopliteModel = unmovedHoplite;
                Debug.Log($"{_hopliteModel.RegionId}");
                _tokenMover.Init(token);
                _tokenMover.CatchNeibRegionPoint(token.RegionId, HandleStep);
            }
        }        
    }
    private void HandleStep(TokenNest nest)
    {
        var regionsRegistry = GameContext.Instance.RegionDataRegistry;
        regionsRegistry.Move(_hopliteModel, nest);
        _hopliteModel.MarkMoved();
        _moveModel.MakeStep();
        _tokenMover.DestroyVisuals();

        if (!_moveModel.CanMove()) {            
            HandleMoveComplete();
        } else {
            _tokenSelector.WaitTokenSelection(_moveModel.PlayerColor, TokenType.HopliteStack, HandleSelection);
        }
    }
    private void HandleMoveComplete()
    {
        Debug.Log("Hoplies Move completed");
    }

    public void OnDone()
    {
        Cleanup();
        _onComplete?.Invoke(RegularActionType.HopliesMove);
    }

    public void OnUndo()
    {
        _moveModel.UndoLast();
    }

    public void OnCancel()
    {
         _moveModel.UndoAll();

        Cleanup();
        _onComplete?.Invoke(RegularActionType.HopliesMove);
    }
     private void Cleanup()
    {
        _moveModel.CanUndoChanged -= _actionControlPanel.SetUndoInteractable;

        _actionControlPanel.Unbind();
        _actionControlPanel.Show(false);
    }
}
