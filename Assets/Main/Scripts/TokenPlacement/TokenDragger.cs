using System;
using UnityEngine;

public class TokenDragger : IDisposable
{
    private UserInputController _userInputController;
    private TokenViewModel _target;

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

        _userInputController.MouseMoved += HandleMouseMove;
    }

    public void Dispose()
    {
        if (_userInputController) {
            _userInputController.MouseMoved -= HandleMouseMove;
        }
    }

    private void HandleMouseMove(Vector2 vector)
    {
        if (_target == null) return;

        AdjustTargetPosition();
    }    

    public void SetTarget(TokenViewModel token)
    {
        if (token is null || _target is not null) {
            Debug.LogWarning("Unabke to SetTarget to TokenDragger!");
            return;
        }
        _target = token;
        AdjustTargetPosition();
    }
    private void AdjustTargetPosition()
    {
        if (_raycastBoard.TryGetBoardPosition(out Vector3 position)) {
            _target.SetWorldPosition(position);
        }
    }
}
