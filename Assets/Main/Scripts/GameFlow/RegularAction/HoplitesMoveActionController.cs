using System;
using UnityEngine;

public class HoplitesMoveActionController : IRegularAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private readonly ActionControlPanel _actionControlPanel;
    private HoplitesMoveActionModel _moveModel;
    private HopliteModel _hopliteModel;
    private Action _onComplete;

    public event Action Completed;

    public HoplitesMoveActionController()
    {
        _actionControlPanel = SceneUIRegistry.Get<ActionControlPanel>();
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        if (_tokenSelector is null || _tokenMover is null) {
            Debug.LogError("HoplitesMoveActionController constructor error!");
        }
    }
    public void Start(HoplitesMoveActionModel moveModel, Action onComplete = null)
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
         _moveModel.UndoAll();

        Cleanup();
        _onComplete?.Invoke();
    }
     private void Cleanup()
    {
        _moveModel.CanUndoChanged -= _actionControlPanel.SetUndoInteractable;

        _actionControlPanel.Unbind();
        _actionControlPanel.Show(false);
    }
}
