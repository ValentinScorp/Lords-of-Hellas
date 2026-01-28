using System;
using UnityEngine;

public class RouteArcBuilder : IDisposable
{
    private RouteArc _arc;
    private TokenViewModel _toToken;
    private UserInputController _userInputController;

    public RouteArc Arc => _arc;
    public RouteArcBuilder()
    {
        _userInputController = ServiceLocator.Get<UserInputController>();
        if (_userInputController == null) {
            Debug.LogError("Unabel to get UserInputController from ServiceLocator!");
            return;
        }
    }
    public void Create(Vector3 fromPosition, TokenViewModel toToken, PlayerColor playerColor)
    {
        _userInputController.MouseMoved += HandleMouseMove;

        _toToken = toToken;
        _arc = new RouteArc();
        _arc.Create(fromPosition, toToken.WorldPosition, playerColor);
    }
    public void SetFirstNode(Vector3 position)
    {
        _arc.SetFirstNode(position);
    }

    private void HandleMouseMove(Vector2 vector)
    {
        _arc.SetSecondNode(_toToken.WorldPosition);
        _arc.BuildArc();
    }       

    public void Dispose()
    {
        _arc.Destroy();
        if (_userInputController) {
            _userInputController.MouseMoved -= HandleMouseMove;
        }
    }

}
