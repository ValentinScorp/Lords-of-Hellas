using System;
using UnityEngine;

internal class TokenDragger : IDisposable
{
    private UserInputController _userInputController;
    private TokenView _ghost;
    internal TokenView Ghost => _ghost;

    private RaycastIntersector _raycastBoard;

    internal TokenDragger()
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
    internal void CreateGhost(TokenModel model)
    {
        if (_userInputController is not null) {
            _userInputController.MouseMoved += OnMouseMove;
        }
        if (_ghost is null) {
            var tokenFactory = ServiceLocator.Get<TokenFactory>();
            _ghost = tokenFactory?.CreateGhostToken(model);
            AdjustGhostPosition();
        }
    }
    public void Dispose()
    {
        if (_ghost is not null) {
            UnityEngine.Object.Destroy(_ghost.gameObject);
            _ghost = null;
        }       

        if (_userInputController is not null) {
            _userInputController.MouseMoved -= OnMouseMove;
        }
    }

    private void OnMouseMove(Vector2 vector)
    {
        if (_ghost == null) return;

        AdjustGhostPosition();
    }    

    private void AdjustGhostPosition()
    {
        if (_raycastBoard.TryGetBoardPosition(out Vector3 position)) {
            _ghost.SetWorldPosition(position);
        }
    }    
}
