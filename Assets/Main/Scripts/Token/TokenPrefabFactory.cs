using UnityEngine;

public class TokenPrefabFactory
{
    private readonly TokenLoader _tokenLoader;
    public TokenPrefabFactory() {
        _tokenLoader = new TokenLoader("Prefabs/Tokens");
    }
    public TokenView CreateToken(TokenView token)
    {
        return CreateTokenView(token.ViewModel.Model);
    }    
    public TokenView CreateTokenView(TokenModel model, Transform parent = null) {
        if (model == null) {
            Debug.Log("Token type is null");
        }
        TokenView tokenView = CreateTokenPrefab(model.Type, parent);
        tokenView.SubscribeOnModel(model);
        return tokenView;
    }
    public float GetRadius(TokenType type) {
        return _tokenLoader.GetRadius(type);
    }

    private TokenView CreateTokenPrefab(TokenType type, Transform parent = null) {
        var prefab = _tokenLoader.GetPrefab(type);
        if (prefab == null) {
            Debug.LogError($"[TokenFactory] Can't create token. Prefab missing for type: {type}");
            return null;
        }
        var token = Object.Instantiate(prefab, parent);
        var tokenView = token.GetComponent<TokenView>();
        if (tokenView == null) {
            Debug.LogWarning("Component didn`t found on prefab!");
        }
        return tokenView;
    }
    
}
