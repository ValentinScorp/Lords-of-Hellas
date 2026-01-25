using System;
using UnityEngine;

public class PlacementController
{
    private TokenModel _token;
    private IRulesChecker _rulesChecker;
    private TokenDragger _tokenDragger = new();
    private TokenNestHitDetector _tokenNestHitDetector = new();
    private Action<TokenModel> _placementCompleted;
    public void Start(TokenModel token, IRulesChecker rulesChecker, Action<TokenModel> onComplete)
    {
        _token = token;
        _rulesChecker = rulesChecker;
        _placementCompleted = onComplete;
        var ghostToken = ServiceLocator.Get<TokenFactory>().CreateGhostToken(_token);
        _tokenDragger.SetTarget(ghostToken);
        _tokenNestHitDetector.ListenHits(HandleHitedNest);
    }
    private void HandleHitedNest(TokenNest nest)
    {
        if (_rulesChecker.CanPlace(_token, nest)) {
            if (GameContext.Instance.RegionDataRegistry.TryPlace(_token, nest)) {
                if (_tokenDragger.TryRemoveTarget(out var tokenVm)) {
                    if (ServiceLocator.Get<TokenViewRegistry>().TryDestroy(tokenVm)) {
                        _placementCompleted(_token);
                        _token = null;
                        _placementCompleted = null;
                        _tokenNestHitDetector.Unlisten();
                        Debug.Log("Token placed!");
                        return;
                    }
                }
            }
        }
    }
}
