using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TokenViewFactory
{
    private readonly TokenLoader _tokenLoader;
    public TokenViewFactory() {
        _tokenLoader = new TokenLoader("Prefabs/Tokens");
    }
    public TokenView CreateTokenView(Token token) {
        if (token == null) {
            Debug.Log("Token type is null");
        }
        TokenView tokenView = CreateTokenPrefab(token.Type);
        tokenView.SubscribeOnModel(token);
        return tokenView;
    }
    public float GetRadius(TokenType type) {
        return _tokenLoader.GetRadius(type);
    }
    private TokenView CreateTokenPrefab(TokenType type) {
        var prefab = _tokenLoader.GetPrefab(type);
        if (prefab == null) {
            Debug.LogError($"[TokenFactory] Can't create token. Prefab missing for type: {type}");
            return null;
        }
        var token = UnityEngine.Object.Instantiate(prefab);
        var tokenView = token.GetComponent<TokenView>();
        if (tokenView == null) {
            Debug.LogWarning("Component didn`t found on prefab!");
        }
        return tokenView;
    }
    
}
