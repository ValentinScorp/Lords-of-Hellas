using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenMover
{
    private TokenHolder _tokenHolder = new();
    private readonly RaycastIntersector _raycastBoard;
    private readonly TerrainValidator _terrainValidator = new();
    private TokenView _originToken;
    private RegionId _prevReionId;
    private Action<TokenNest> _onComplete;
    private readonly List<RouteLink> _routeLinks = new();
    private RouteLink _currentRouteLink;

    public TokenMover()
    {
        _raycastBoard = ServiceLocator.Get<RaycastIntersector>();
    }
    public void CatchNeibRegionPoint(TokenView token, RegionId fromRegion, Action<TokenNest> onComplete, Action onCancel = null)    
    {
        _originToken = token;
        _prevReionId = fromRegion;
        _onComplete = onComplete;

        ServiceLocator.Get<ObjectsHitDetector>().Listen(HandleSelections);
    }
    private void HandleSelections(List<ObjectsHitDetector.Target> targets)
    {
        if (_originToken == null || targets == null) return;

        foreach (var target in targets){
            if (target.Selectable is RegionAreaView regionArea) {
                RegionHited(regionArea, target.HitPoint);
                break;
            }
        }
    }
    private void RegionHited(RegionAreaView region, Vector3 hitPoint)
    {
        var regRegistry = GameContext.Instance.RegionDataRegistry;

        var neibRegions = regRegistry.GetNeighborRegionIds(_prevReionId);
        foreach (var regId in neibRegions) {
            if (region.Id == regId) {
                ServiceLocator.Get<ObjectsHitDetector>().Unlisten();
                var regionsView = ServiceLocator.Get<RegionsView>();
                var spawnPoint = regionsView.GetFreeSpawnPoint(region.Id, hitPoint);
                _currentRouteLink?.SetFirstNode(spawnPoint.Position);
                _onComplete?.Invoke(spawnPoint);
                break;
            }
        }        
    }
    public void Update()
    {
        if (!_tokenHolder.HasObject()) return;
        if (_raycastBoard.TryGetBoardPosition(out Vector3 newPosition)) {
            if (_tokenHolder.GetGameObjectPosition() != newPosition) {
                _tokenHolder.SetGameObjectPosition(newPosition);
                var state = _terrainValidator.ValidatePlacement(newPosition);
                _tokenHolder.TokenView.SetGhostColor(state);
                _currentRouteLink?.SetSecondNode(newPosition);
                _currentRouteLink?.BuildArc();
            }
        }
    }
    public void CreateGhostToken(TokenView token)
    { 
        var tokenFactory = ServiceLocator.Get<TokenFactory>();
        var ghostToken = tokenFactory.CreateGhostToken(token.ViewModel.Model);
        // _tokenHolder.AttachToken(ghostToken);
        float radius = tokenFactory.GetRadius(token.TokenType);
        _terrainValidator.SetTokenRadius(radius);
        var routeLink = new RouteLink();
        routeLink.Create(token.transform.position, ghostToken.WorldPosition, token.PlayerColor);
        _currentRouteLink = routeLink;
        _routeLinks.Add(routeLink);
    }
    public void DestroyVisuals()
    {
        foreach (var routeLink in _routeLinks) {
            routeLink.Destroy();
        }
        _routeLinks.Clear();
        _currentRouteLink = null;
        _tokenHolder.DestroyTokenView();
    }
    public bool CanPlaceToken(out RegionId regionId) {
        regionId = RegionId.Unknown;
        if (!_tokenHolder.HasObject() || !_terrainValidator.IsCurrentStateAllowed) {
            return false;
        }
        var position = _tokenHolder.GetGameObjectPosition();
        if (!position.HasValue) 
            return false;

        if (!_terrainValidator.TryGetRegionIdAtPosition(position.Value, out regionId)) {
            Debug.Log("Can't place: region not found at token position.");
            return false;
        }        
        return true;
    }
}
