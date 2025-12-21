using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenSelector
{
    private bool _waitingToken;
    private TokenType _tokenType;
    private PlayerColor _playerColor;
    private Action<TokenView> _handleSelection; 
    public void WaitTokenSelection(PlayerColor playerColor, TokenType type, Action<TokenView> handleToken)
    {
        Debug.Log($"[TokenSelector] WaitTokenSelection playerColor={playerColor}, tokenType={type}");
        _waitingToken = true;
        _playerColor = playerColor;
        _tokenType = type;
        _handleSelection = handleToken;
        ServiceLocator.Get<SelectMgr>().ListenSelection(HandleSelectables);
    }

    private void HandleSelectables(List<ISelectable> selectables)
    {
        if (!_waitingToken || selectables == null) return;

        foreach (var selectable in selectables) {
            if (selectable is TokenView token) {
                if (token.PlayerColor == _playerColor &&  token.TokenType == _tokenType)  {
                    ServiceLocator.Get<SelectMgr>().UnlistenSelection();
                    _waitingToken = false;
                    _handleSelection?.Invoke(token);
                    _handleSelection = null;
                    break;
                }
            }
        }
    }
}
