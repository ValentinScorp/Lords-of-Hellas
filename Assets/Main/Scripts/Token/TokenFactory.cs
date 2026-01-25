using UnityEngine;

public class TokenFactory
{
    private readonly TokenLoader _tokenLoader;
    public TokenFactory() {
        _tokenLoader = new TokenLoader("Prefabs/Tokens");
    }
    public TokenViewModel CreateGhostToken(TokenModel model)
    {
        return CreateTokenViewModel(model);        
    }

    public TokenViewModel CreateTokenViewModel(TokenModel model, Transform parent = null) {
        if (model == null) {
            Debug.Log("Can't create toke prefab! Token model is null!");
            return null;
        }
        var view = CreateTokenPrefab(model, parent);
        var viewModel = CreateViewModel(model);
        view.SubscribeToViewModel(viewModel);
        ServiceLocator.Get<TokenViewRegistry>().Register(viewModel, view);
        return viewModel;
    }
    public float GetRadius(TokenType type) {
        return _tokenLoader.GetRadius(type);
    }

    private TokenView CreateTokenPrefab(TokenModel model, Transform parent = null) {
        var prefab = _tokenLoader.GetPrefab(model.Type);
        if (prefab == null) {
            Debug.LogError($"[TokenFactory] Can't create token. Prefab missing for type: {model.Type}");
            return null;
        }
        var token = Object.Instantiate(prefab, parent);
        var tokenView = token.GetComponent<TokenView>();
        if (tokenView == null) {
            Debug.LogWarning("Component didn`t found on prefab!");
        }
        return tokenView;
    }
    private TokenViewModel CreateViewModel(TokenModel model)
    {
        return model switch
        {
            HeroModel hero => new HeroViewModel(hero),
            HopliteStackModel hoplite => new HopliteStackViewModel(hoplite),
            _ => new TokenViewModel(model),
        };
    }
}
