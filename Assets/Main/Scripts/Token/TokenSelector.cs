using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class TokenSelector
{
    private bool _waitingToken;
    private TokenType _tokenType;
    private PlayerColor _playerColor;
    private Action<TokenView> _handleSelection; 
    public void WaitTokenSelection(PlayerColor playerColor, TokenType type, Action<TokenView> handleToken)
    {
        //Debug.Log($"[TokenSelector] WaitTokenSelection playerColor={playerColor}, tokenType={type}");
        _waitingToken = true;
        _playerColor = playerColor;
        _tokenType = type;
        _handleSelection = handleToken;
        ServiceLocator.Get<ClickMgr>().ListenClicks(HandleClickables);
    }

    private void HandleClickables(List<IClickable> clickables)
    {
        if (!_waitingToken || clickables == null) return;

        foreach (var clickable in clickables) {
            if (clickable is TokenView token) {
                if (token.PlayerColor == _playerColor &&  token.TokenType == _tokenType)  {
                    ServiceLocator.Get<ClickMgr>().UnlistenClicks();
                    _waitingToken = false;
                    _handleSelection?.Invoke(token);
                    _handleSelection = null;
                    break;
                }
            }
        }
    }
}
