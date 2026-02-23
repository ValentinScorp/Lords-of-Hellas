using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenMover
{
    private RegionId _prevReionId;
    private TokenDragger _tokenDragger;
    private TokenNestHitDetector _tokenNestHitDetector;
    private Action<RegionNest> _onComplete;
    private readonly List<RouteArc> _arcsList = new();
    private RouteArcBuilder _arcBuilder;

    public TokenMover()
    {
        _tokenDragger = new();
        _tokenNestHitDetector = new();
    }
    public void Init(TokenView token)
    {         
        _tokenDragger.CreateGhost(token.ViewModel.Model);

        var arcBuilder = new RouteArcBuilder();
        arcBuilder.Create(token.transform.position, _tokenDragger.Ghost, token.PlayerColor);
        _arcBuilder = arcBuilder;
        _arcsList.Add(arcBuilder.Arc);
    }
    public void CatchNeibRegionPoint(RegionId fromRegion, Action<RegionNest> onComplete, Action onCancel = null)    
    {
        _prevReionId = fromRegion;
        _onComplete = onComplete;

        _tokenNestHitDetector.ListenHits(HandleHittedNest);
    }
    private void HandleHittedNest(RegionNest nest)
    {
        var regRegistry = GameContext.Instance.RegionRegistry;

        var neibRegions = regRegistry.GetNeighborRegionIds(_prevReionId);
        foreach (var regId in neibRegions) {
            if (nest.RegionId == regId) {
                _tokenNestHitDetector.Unlisten();

                _arcBuilder?.SetFirstNode(nest.Position);
                _onComplete?.Invoke(nest);
            }
        }
    }
    
    public void DestroyVisuals()
    {
        foreach (var routeLink in _arcsList) {
            routeLink.Destroy();
        }
        _arcsList.Clear();
        _arcBuilder.Dispose();
        _arcBuilder = null;
        _tokenDragger.Dispose();   
    }
}
