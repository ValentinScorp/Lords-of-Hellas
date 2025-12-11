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
    private System.Action<RegionId> _onComplete;
    public TokenMover()
    {
        _raycastBoard = ServiceLocator.Get<RaycastIntersector>();
    }
    public void StartMove(TokenView token, System.Action<RegionId> onComplete, System.Action onCancel = null)    
    {
        _originToken = token;
        _onComplete = onComplete;
        CreateGhostToken(token);

        ServiceLocator.Get<ClickMgr>().ListenClicks(HandleClickables);

        var regRegistry = ServiceLocator.Get<RegionStatusRegistry>();
        RegionId regionId = token.Model.RegionId;

        var neibRegions = regRegistry.GetNeighborRegionIds(regionId);
        foreach (var regId in neibRegions) {
            // Debug.Log($"Neighbor region: {regId}");  
        }        
    }
    private void HandleClickables(List<IClickable> clickables)
    {
        if (_originToken == null || clickables == null) return;

        foreach (var clickable in clickables){
            if (clickable is RegionAreaView regionArea) {
                RegionClicked(regionArea.RegionId);
                break;
            }
        }
    }
    private void RegionClicked(RegionId regionId)
    {
        var regRegistry = ServiceLocator.Get<RegionStatusRegistry>();
        RegionId originRegionId = _originToken.Model.RegionId;

        var neibRegions = regRegistry.GetNeighborRegionIds(originRegionId);
        foreach (var regId in neibRegions) {
            if (regionId == regId) {
                ServiceLocator.Get<ClickMgr>().UnlistenClicks();
                _onComplete?.Invoke(regionId);
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
                _line?.SetPosition(1, newPosition);
            }
        }
    }
    private void CreateGhostToken(TokenView token)
    { 
        var tokenFactory = ServiceLocator.Get<TokenPrefabFactory>();
        var ghostToken = tokenFactory.CreateToken(token);
        _tokenHolder.AttachToken(ghostToken);
        float radius = tokenFactory.GetRadius(token.TokenType);
        _terrainValidator.SetTokenRadius(radius);

        Color lineColor = GameData.Instance.GetPlayerColor(token.PlayerColor);
        _line = new GameObject("MoveLine").AddComponent<LineRenderer>();
        // _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.positionCount = 2;
        _line.startColor = _line.endColor = lineColor;
        _line.useWorldSpace = true;
        _line.startWidth = _line.endWidth = 0.05f; // підбери під себе
        _line.SetPosition(0, token.transform.position);
        _line.SetPosition(1, ghostToken.transform.position);    
    }
}
