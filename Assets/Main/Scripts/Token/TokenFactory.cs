using UnityEngine;

public class TokenFactory
{
    private readonly TokenLoader _tokenLoader;
    public TokenFactory() {
        _tokenLoader = new TokenLoader("Prefabs/Tokens");
    }
    public TokenView CreateGhostToken(TokenModel model)
    {
        return CreateTokenPrefab(model);        
    }

    public float GetRadius(TokenType type) {
        return _tokenLoader.GetRadius(type);
    }

    public TokenView CreateTokenPrefab(TokenModel model, Transform parent = null) {
        var prefab = _tokenLoader.GetPrefab(model.Type);
        if (prefab == null) {
            Debug.LogError($"[TokenFactory] Can't create token. Prefab missing for type: {model.Type}");
            return null;
        }
        var token = Object.Instantiate(prefab, parent);
        var tokenView = token.GetComponent<TokenView>();
        if (tokenView == null) {
            Debug.LogWarning("Component didn`t found on prefab!");
        } else {
            tokenView.SubscribeToModel(model);
        }
        return tokenView;
    }
}
