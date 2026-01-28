using System;
using Mono.Cecil.Cil;
using UnityEngine;

public class TokenDragger : IDisposable
{
    private UserInputController _userInputController;
    private TokenViewModel _ghost;
    public TokenViewModel Ghost => _ghost;

    private RaycastIntersector _raycastBoard;

    public TokenDragger()
    {
        _userInputController = ServiceLocator.Get<UserInputController>();
        if (_userInputController == null) {
            Debug.LogError("Unabel to get UserInputController from ServiceLocator!");
            return;
        }
        _raycastBoard = ServiceLocator.Get<RaycastIntersector>();
        if (_raycastBoard == null) {
            Debug.LogError("Unabel to get RaycastIntersector from ServiceLocator!");
            return;
        }        
    }
    public void CreateGhost(TokenModel model)
    {
        _userInputController.MouseMoved += HandleMouseMove;

        var tokenFactory = ServiceLocator.Get<TokenFactory>();
        _ghost = tokenFactory.CreateGhostToken(model);
        AdjustGhostPosition();
    }
    public void Dispose()
    {
        var viewRegistry = ServiceLocator.Get<TokenViewRegistry>();
        if (viewRegistry is null) {
            Debug.LogWarning("Unable to get TokenViewRegistry!");
        }
        if(!viewRegistry.TryDestroy(_ghost)) {
            Debug.LogWarning("Unable to destroy ghost TokenView in TokenDragger!");
        } else {      
            _ghost = null;
        }
        if (_userInputController) {
            _userInputController.MouseMoved -= HandleMouseMove;
        }
    }

    private void HandleMouseMove(Vector2 vector)
    {
        if (_ghost == null) return;

        AdjustGhostPosition();
    }    

    // public void SetTarget(TokenViewModel token)
    // {
    //     if (token is null || _ghost is not null) {
    //         Debug.LogWarning("Unable to SetTarget to TokenDragger!");
    //         return;
    //     }
    //     _ghost = token;
    //     AdjustTargetPosition();
    // } 
    private void AdjustGhostPosition()
    {
        if (_raycastBoard.TryGetBoardPosition(out Vector3 position)) {
            _ghost.SetWorldPosition(position);
        }
    }    
}
