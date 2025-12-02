using System;
using UnityEngine;

public class TokenPlacementManager
{
    private readonly TokenHolder _tokenHolder = new();
    private readonly TokenPlacementTracker _tokenPlacementTracker = new TokenPlacementTracker();
    private readonly TokenModelFactory _tokenModelFactory;
    private readonly TerrainValidator _terrainValidator = new();
    private readonly TokenPlacementRulesValidator _rulesValidator = new();
    private readonly RegionViewManager _regionVisuals;
    private TokenVisualChanger _tokenVisualChanger;
    private RegionDataManager _regionManager;
    private TokenPlacementRecorder _recorder;
    private int _startupPlacementCounter = 0;
    private const int StartupPlacementCounterMax = 3;
    private TokenModel _currentToken;

    public TokenPlacementTracker TokenPlacementTracker => _tokenPlacementTracker;

    public event Action OnPlacementStarted;
    public event Action OnPlacementCompleted;
    public event Action OnTokenPlaced;

    private const int MaxStartupPlacements = 3;

    public TokenPlacementManager(RegionDataManager regionManager, RegionViewManager regionVisuals) {
        _tokenModelFactory = new TokenModelFactory();
        _regionManager = regionManager;
        _regionVisuals = regionVisuals;
        _tokenVisualChanger = new TokenVisualChanger(GameData.TokenMaterialPalette);
        _recorder = new();
    }
    public void InitiatePlacing(Player player) {
        _tokenPlacementTracker.SetPlacementTargets(player);
        _startupPlacementCounter = 0;
        _currentToken = null;
        OnPlacementStarted?.Invoke();
    }
    private void OnHeroBonusCompleted() {
        OnPlacementCompleted?.Invoke();        
    }
    public bool StartPlacingToken(TokenType tokenType) {
        if (_tokenHolder.HasObject()) {
            _tokenHolder.DestroyTokenView();
        }
        if (!_tokenPlacementTracker.CanPlace(tokenType)) {
            Debug.Log($"[TokenPlacementManager] Cannot start placing {tokenType} — limit reached");
            return false;
        }
        _currentToken = _tokenPlacementTracker.TakeToken(tokenType);
        if (_currentToken == null) {
            Debug.LogWarning($"[TokenPlacementManager] No available token for type {tokenType}");
            return false;
        }

        var tokenPrefabFactory = ServiceLocator.Get<TokenPrefabFactory>();
        TokenView tokenView = tokenPrefabFactory.CreateTokenView(_currentToken);

        _tokenHolder.AttachToken(tokenView);

        float radius = tokenPrefabFactory.GetRadius(tokenType);
        _terrainValidator.SetTokenRadius(radius);

        return true;
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
    public bool TryPlace() {
        if (!_tokenHolder.HasObject() || !_terrainValidator.IsCurrentStateAllowed) {
            //Debug.Log("Can't place: no token held or placement not allowed.");
            return false;
        }
        var position = _tokenHolder.GetGameObjectPosition();
        if (!position.HasValue) return false;

        if (!_terrainValidator.TryGetRegionIdAtPosition(position.Value, out RegionId regionId)) {
            Debug.Log("Can't place: region not found at token position.");
            return false;
        }
        if (!_rulesValidator.ValidateLogicalPlacement(_regionManager, regionId, _currentToken)) {
            return false;
        }
        PlaceToken(regionId, GameState.Instance.CurrentPlayer);
        OnTokenPlaced?.Invoke();
        return true;
    }
    private void PlaceToken(RegionId regionId, Player player) {
        RecordStep(regionId, player.Color, _currentToken.Type);
        _regionManager.RegisterEntity(regionId, _currentToken);
        HandleVisuals(_currentToken.Type, regionId, player.Color);
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
                int hopliteCount = _regionManager.GetHopliteNum(regionId, color);
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
            _regionManager.UnregisterToken(_recorder.LastStepRegionId, _recorder.LastStepTokenType, _recorder.LastStepPlayerColor);
            _regionVisuals.RemoveToken(_recorder.LastStepRegionId, _recorder.LastStepTokenType, _recorder.LastStepPlayerColor);
            _recorder.RemoveLastStep();
        }
        _startupPlacementCounter = 0;
        _currentToken = null;
        _tokenHolder.DestroyTokenView();        
        _tokenPlacementTracker.Reset();
    } 
}
