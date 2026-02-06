using System;
using UnityEngine;

public class HoplitesMoveActionController : IRegularAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private HoplitesMoveActionModel _moveModel;
    private HopliteModel _hopliteModel;
    private Action _onComplete;

    private readonly ActionControlPanelController _actionControlPanelController;

    public event Action Completed;

    public HoplitesMoveActionController()
    {
        _actionControlPanelController = ServiceLocator.Get<ActionControlPanelController>();
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
        _actionControlPanelController.Start(this);
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
