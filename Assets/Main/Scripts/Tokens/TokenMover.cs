using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class TokenMover
{
    private TokenHolder _tokenHolder = new();
    private readonly RaycastIntersector _raycastBoard;
    private readonly TerrainValidator _terrainValidator = new();
    private TokenView _originToken;
    private System.Action _onComplete;
    public TokenMover()
    {
        _raycastBoard = ServiceLocator.Get<RaycastIntersector>();
    }
    public void StartMove(TokenView token, System.Action onComplete)    
    {
        _originToken = token;
        _onComplete = onComplete;
        CreateGhostToken(token);
    }
    public void Update()
    {
        if (!_tokenHolder.HasObject()) return;
        if (_raycastBoard.TryGetBoardPosition(out Vector3 newPosition)) {
            if (_tokenHolder.GetGameObjectPosition() != newPosition) {
                _tokenHolder.SetGameObjectPosition(newPosition);
                var state = _terrainValidator.ValidatePlacement(newPosition);
                _tokenHolder.TokenView.SetGhostColor(state);
            }
        }
    }
    private void CreateGhostToken(TokenView token)
    { 
        var tokenFactory = ServiceLocator.Get<TokenPrefabFactory>();
        var copyToken = tokenFactory.CreateToken(token);
        _tokenHolder.AttachToken(copyToken);
        float radius = tokenFactory.GetRadius(token.TokenType);
        _terrainValidator.SetTokenRadius(radius);        
    }
}
