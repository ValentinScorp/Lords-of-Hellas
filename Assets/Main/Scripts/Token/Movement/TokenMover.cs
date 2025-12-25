using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenMover
{
    private TokenHolder _tokenHolder = new();
    private readonly RaycastIntersector _raycastBoard;
    private readonly TerrainValidator _terrainValidator = new();
    private LineRenderer _line;
    private static Material _lineMaterial;
    private TokenView _originToken;
    private RegionId _prevReionId;
    private Action<SpawnPoint> _onComplete;
    private RouteLink _routeLink = new RouteLink();

    public TokenMover()
    {
        _raycastBoard = ServiceLocator.Get<RaycastIntersector>();
    }
    public void ProceedStep(TokenView token, RegionId fromRegion, Action<SpawnPoint> onComplete, Action onCancel = null)    
    {
        _originToken = token;
        _prevReionId = fromRegion;
        _onComplete = onComplete;

        ServiceLocator.Get<SelectMgr>().ListenTokenHits(HandleSelections);

        var regRegistry = ServiceLocator.Get<RegionDataRegistry>();
        RegionId regionId = token.Model.RegionId;

        var neibRegions = regRegistry.GetNeighborRegionIds(regionId);
        foreach (var regId in neibRegions) {
            // Debug.Log($"Neighbor region: {regId}");  
        }        
    }
    private void HandleSelections(List<SelectMgr.Target> targets)
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
        var regRegistry = ServiceLocator.Get<RegionDataRegistry>();

        var neibRegions = regRegistry.GetNeighborRegionIds(_prevReionId);
        foreach (var regId in neibRegions) {
            if (region.RegionId == regId) {
                ServiceLocator.Get<SelectMgr>().UnlistenTokneSelection();
                var regionsView = ServiceLocator.Get<RegionsView>();
                var spawnPoint = regionsView.GetFreeSpawnPoint(region.RegionId, hitPoint);
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
                _routeLink?.SetSecondNode(newPosition);
                _routeLink?.BuildArc();
            //     _line?.SetPosition(1, newPosition);
            }
        }
    }
    public void CreateGhostToken(TokenView token)
    { 
        var tokenFactory = ServiceLocator.Get<TokenPrefabFactory>();
        var ghostToken = tokenFactory.CreateToken(token);
        _tokenHolder.AttachToken(ghostToken);
        float radius = tokenFactory.GetRadius(token.TokenType);
        _terrainValidator.SetTokenRadius(radius);
        _routeLink.Create(token.transform.position, ghostToken.transform.position, token.PlayerColor);  
    }
    public void DestroyVisuals()
    {
        _routeLink.Destroy();
        _tokenHolder.DestroyTokenView();
    }
}
