using UnityEngine;

internal class TokenFactory
{
    private readonly TokenLoader _tokenLoader;
    internal TokenFactory() {
        _tokenLoader = new TokenLoader("Prefabs/Tokens");
    }
    internal TokenView CreateGhostToken(TokenModel model)
    {
        return CreateTokenPrefab(model);        
    }

    internal float GetRadius(TokenType type) {
        return _tokenLoader.GetRadius(type);
    }

    internal TokenView CreateTokenPrefab(TokenModel model, Transform parent = null) {
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
