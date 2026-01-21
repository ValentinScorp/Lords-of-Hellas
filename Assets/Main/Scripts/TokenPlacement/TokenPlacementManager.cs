using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenPlacementManager
{
    private readonly TokenHolder _tokenHolder = new();
    private readonly TokenPlacementPool _tokenPlacementPool = new TokenPlacementPool();
    private readonly TerrainValidator _terrainValidator = new();
    private readonly TokenPlacementRulesValidator _rulesValidator = new();
    private readonly RegionsView _regionsView;
    
    private RegionsContext _regionDataRegistry;
    private TokenPlacementRecorder _recorder;
    private int _startupPlacementCounter = 0;
    private const int StartupPlacementCounterMax = 3;
    private Player _currentPlayer;
    private TokenModel _currentToken;

    public TokenPlacementPool TokenPlacementPool => _tokenPlacementPool;

    public event Action OnPlacementStarted;
    public event Action OnPlacementCompleted;
    public event Action OnTokenPlaced;

    private const int MaxStartupPlacements = 3;

    public TokenPlacementManager(RegionsView regionVisuals) {
        _regionDataRegistry = GameContext.Instance.RegionDataRegistry;
        _regionsView = regionVisuals;
        
        _recorder = new();
    }
    public void InitiatePlacing(Player player) {
        _currentPlayer = player;
        _tokenPlacementPool.SetPlacementTargets(player);
        _startupPlacementCounter = 0;
        _currentToken = null;
        OnPlacementStarted?.Invoke();
    }
    private void OnHeroBonusCompleted() {
        OnPlacementCompleted?.Invoke();        
    }
    
    public void UpdatePlacement(RaycastIntersector raycastBoard) {
        if (!_tokenHolder.HasObject()) return;
        if (raycastBoard.TryGetBoardPosition(out Vector3 newPosition)) {
            if (_tokenHolder.GetGameObjectPosition() != newPosition) {
                _tokenHolder.SetGameObjectPosition(newPosition);
                var state = _terrainValidator.ValidatePlacement(newPosition);
                _tokenHolder.TokenView.SetGhostColor(state);
            }
        }
    }
    private bool CanPlaceToken(out RegionId regionId) {
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
        if (!_rulesValidator.ValidateLogicalPlacement(_regionDataRegistry, regionId, _currentToken)) {
            return false;
        }
        return true;
    }
    
    private void RecordStep(RegionId regionId, PlayerColor color, TokenType tokenType) {
        var position = _tokenHolder.GetGameObjectPosition();
        if (position.HasValue) _recorder.AddStep(color, tokenType, regionId, position.Value);
        _startupPlacementCounter++;
    }
    
    public void FinalizePlacement() {
        _tokenHolder.DestroyTokenView();
        _currentToken = null;
        _startupPlacementCounter = 0;
        GameContext.Instance.CurrentPlayer.ApplyHeroStartingBonus(OnHeroBonusCompleted);        
    }
    public void Cancel() {
        for (int i=0; i < _startupPlacementCounter; i++) {
            _regionDataRegistry.UnregisterToken(_recorder.LastStepRegionId, _recorder.LastStepTokenType, _recorder.LastStepPlayerColor);
            _regionsView.RemoveToken(_recorder.LastStepRegionId, _recorder.LastStepTokenType, _recorder.LastStepPlayerColor);
            _recorder.RemoveLastStep();
        }
        _startupPlacementCounter = 0;
        _currentToken = null;
        _tokenHolder.DestroyTokenView();        
        _tokenPlacementPool.Reset();
        InitiatePlacing(_currentPlayer);
    } 
    public void StartPlacingToken(TokenType tokenType) {
        if (_tokenHolder.HasObject()) {
            return;
        }
        if (!_tokenPlacementPool.CanPlace(tokenType)) {
            Debug.Log($"[TokenPlacementManager] Cannot start placing {tokenType} — limit reached");
            return;
        }
        _currentToken = _tokenPlacementPool.TakeToken(tokenType);
        if (_currentToken == null) {
            Debug.LogWarning($"[TokenPlacementManager] No available token for type {tokenType}");
            return;
        }
        var tokenPrefabFactory = ServiceLocator.Get<TokenFactory>();
        // TokenView tokenView = tokenPrefabFactory.CreateTokenView(_currentToken);

        // _tokenHolder.AttachToken(tokenView);

        float radius = tokenPrefabFactory.GetRadius(tokenType);
        _terrainValidator.SetTokenRadius(radius);
        ServiceLocator.Get<ObjectsHitDetector>().Listen(HandleHits);
    }
    private void HandleHits(List<ObjectsHitDetector.Target> targets)
    {
        if (_currentToken == null || targets == null) return;

        foreach (var t in targets) {
            if (t.Selectable is RegionAreaView regionArea) {
                if (CanPlaceToken(out RegionId regionId)) {
                    PlaceToken(regionId);
                    ServiceLocator.Get<ObjectsHitDetector>().Unlisten();
                }                
                break;
            }
        }
    }
    private void PlaceToken(RegionId regionId) {
        PlayerColor color = GameContext.Instance.CurrentPlayer.Color;
        RecordStep(regionId, color, _tokenHolder.TokenView.TokenType);
        HandleVisuals(_tokenHolder.TokenView.TokenType, regionId, color);
        
        _regionDataRegistry.RegisterToken(regionId, _currentToken);
        OnTokenPlaced?.Invoke();
        _currentToken = null;
    }
    private void HandleVisuals(TokenType tokenType, RegionId regionId, PlayerColor color) {
        if (tokenType == TokenType.HopliteStack) {
            var existingHoplite = _regionsView.GetHopliteFromRegion(regionId, color);
            if (existingHoplite != null) {
                int hopliteCount = _regionDataRegistry.GetHopliteNum(regionId, color);
                existingHoplite.GetComponent<TokenView>()?.SetCount(hopliteCount);
                _tokenHolder.DestroyTokenView();
                _tokenHolder.UnattachToken();
                return;
            } 
        }
        SpawnPoint spawn = _regionsView.PlaceToken(_tokenHolder.TokenView, regionId, _tokenHolder.GetGameObjectPosition());
        if (spawn != null) {
            ServiceLocator.Get<TokenVisualChanger>().PrepareTokenPlacement(_tokenHolder.TokenView, color);
            _tokenHolder.SetGameObjectPosition(spawn.Position);
            _tokenHolder.UnattachToken();
        }
    }    
}
