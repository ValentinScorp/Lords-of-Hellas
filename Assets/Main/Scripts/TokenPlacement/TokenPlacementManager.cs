using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenPlacementManager
{
    private readonly TokenHolder _tokenHolder = new();
    private readonly TokenPlacementPool _tokenPlacementPool = new TokenPlacementPool();
    private readonly TokenModelFactory _tokenModelFactory;
    private readonly TerrainValidator _terrainValidator = new();
    private readonly TokenPlacementRulesValidator _rulesValidator = new();
    private readonly RegionsView _regionVisuals;
    private TokenVisualChanger _tokenVisualChanger;
    private RegionDataRegistry _regionStatusRegistry;
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
        _tokenModelFactory = new TokenModelFactory();
        _regionStatusRegistry = ServiceLocator.Get<RegionDataRegistry>();
        _regionVisuals = regionVisuals;
        _tokenVisualChanger = new TokenVisualChanger(GameData.TokenMaterialPalette);
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

        var tokenPrefabFactory = ServiceLocator.Get<TokenPrefabFactory>();
        TokenView tokenView = tokenPrefabFactory.CreateTokenView(_currentToken);

        _tokenHolder.AttachToken(tokenView);

        float radius = tokenPrefabFactory.GetRadius(tokenType);
        _terrainValidator.SetTokenRadius(radius);
        ServiceLocator.Get<SelectMgr>().ListenSelection(HandleClickables);
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
        if (!_rulesValidator.ValidateLogicalPlacement(_regionStatusRegistry, regionId, _currentToken)) {
            return false;
        }
        return true;
    }
    private void PlaceToken(RegionId regionId) {
        PlayerColor color = GameState.Instance.CurrentPlayer.Color;
        RecordStep(regionId, color, _tokenHolder.TokenView.TokenType);
        _regionStatusRegistry.RegisterEntity(regionId, _currentToken);
        HandleVisuals(_tokenHolder.TokenView.TokenType, regionId, color);
        OnTokenPlaced?.Invoke();
        _currentToken = null;
    }
    private void RecordStep(RegionId regionId, PlayerColor color, TokenType tokenType) {
        var position = _tokenHolder.GetGameObjectPosition();
        if (position.HasValue) _recorder.AddStep(color, tokenType, regionId, position.Value);
        _startupPlacementCounter++;
    }
    private void HandleVisuals(TokenType tokenType, RegionId regionId, PlayerColor color) {
        if (tokenType == TokenType.HopliteStack) {
            var existingHoplite = _regionVisuals.GetHopliteFromRegion(regionId, color);
            if (existingHoplite != null) {
                int hopliteCount = _regionStatusRegistry.GetHopliteNum(regionId, color);
                existingHoplite.GetComponent<TokenView>()?.SetCount(hopliteCount);
                _tokenHolder.DestroyTokenView();
                _tokenHolder.UnattachToken();
                return;
            }
        }

        SpawnPoint spawn = _regionVisuals.PlaceToken(_tokenHolder.TokenView.gameObject, regionId, _tokenHolder.GetGameObjectPosition());
        if (spawn != null) {
            _tokenVisualChanger.PrepareTokenPlacement(_tokenHolder.TokenView, color);
            _tokenHolder.SetGameObjectPosition(spawn.Position);
            _tokenHolder.UnattachToken();
        }
    }    
    public void FinalizePlacement() {
        _tokenHolder.DestroyTokenView();
        _currentToken = null;
        _startupPlacementCounter = 0;
        GameState.Instance.CurrentPlayer.ApplyHeroStartingBonus(OnHeroBonusCompleted);        
    }
    public void Cancel() {
        for (int i=0; i < _startupPlacementCounter; i++) {
            _regionStatusRegistry.UnregisterToken(_recorder.LastStepRegionId, _recorder.LastStepTokenType, _recorder.LastStepPlayerColor);
            _regionVisuals.RemoveToken(_recorder.LastStepRegionId, _recorder.LastStepTokenType, _recorder.LastStepPlayerColor);
            _recorder.RemoveLastStep();
        }
        _startupPlacementCounter = 0;
        _currentToken = null;
        _tokenHolder.DestroyTokenView();        
        _tokenPlacementPool.Reset();
        InitiatePlacing(_currentPlayer);
    } 
    private void HandleClickables(List<ISelectable> clickables)
    {
        if (_currentToken == null || clickables == null) return;

        foreach (var clickable in clickables) {
            if (clickable is RegionAreaView regionArea) {
                if (CanPlaceToken(out RegionId regionId)) {
                    PlaceToken(regionId);
                    ServiceLocator.Get<SelectMgr>().UnlistenSelection();
                }                
                break;
            }
        }
    }
}
