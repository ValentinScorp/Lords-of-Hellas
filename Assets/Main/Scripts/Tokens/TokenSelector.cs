using System;
using UnityEngine;
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
    }
    public void HandleTokenClick(TokenView token)
    {
        // Debug.Log($"[TokenSelector] HandleTokenClick waiting={_waitingToken}, tokenColor={token.PlayerColor}, tokenType={token.TokenType}");
        if (!_waitingToken) return;
        if (_playerColor != token.PlayerColor) return;
        if (_tokenType != token.TokenType) return;
        // Debug.Log("HandleTokenClick guard pass!");
        _waitingToken = false;
        _handleSelection?.Invoke(token);
        _handleSelection = null;
    }
}
