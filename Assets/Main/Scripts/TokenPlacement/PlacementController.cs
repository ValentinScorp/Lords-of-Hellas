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
        _tokenDragger.CreateGhost(_token);
        _tokenNestHitDetector.ListenHits(HandleHitedNest);
    }
    private void HandleHitedNest(RegionNest nest)
    {
        if (_rulesChecker.CanPlace(_token, nest)) {            
            if (GameContext.Instance.RegionRegistry.TryPlace(_token, nest)) {
                _tokenDragger.Dispose();
                _placementCompleted(_token);
                _token = null;
                _placementCompleted = null;
                _tokenNestHitDetector.Unlisten();                
            }
        }
    }
}
