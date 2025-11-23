using System;
using UnityEngine;

public class TokenPlacementManager
{
    private readonly TokenHolder _tokenHolder = new();
    private readonly TokenPlacementTracker _tracker = new TokenPlacementTracker();
    private readonly TokenModelFactory _tokenModelFactory;
    private readonly TokenViewFactory _tokenViewFactory;
    private readonly TokenPlacementTerrainValidator _terrainValidator = new();
    private readonly TokenPlacementRulesValidator _rulesValidator = new();
    private readonly RegionViewManager _regionVisuals;
    private TokenVisualChanger _tokenVisualChanger;
    private RegionDataManager _regionManager;
    private TokenPlacementRecorder _recorder;
    private int _startupPlacementCounter = 0;
    private const int StartupPlacementCounterMax = 3;
    private Token _currentToken;

    public TokenPlacementTracker TokenPlacementTracker => _tracker;

    public event Action OnPlacementStarted;
    public event Action OnPlacementCompleted;
    public event Action OnTokenPlaced;

    private const int MaxStartupPlacements = 3;

    public TokenPlacementManager(RegionDataManager regionManager, RegionViewManager regionVisuals) {
        _tokenModelFactory = new TokenModelFactory();
        _tokenViewFactory = new TokenViewFactory();
        _regionManager = regionManager;
        _regionVisuals = regionVisuals;
        _tokenVisualChanger = new TokenVisualChanger(GameData.TokenMaterialPalette);
        _recorder = new();
    }
    public void InitiatePlacing(Player player, int maxHeroes, int maxHoplites) {
        GameState.Instance.CurrentPlayer.TakeCombatCards(1);
        _tracker.SetPlacementTargets(maxHeroes, maxHoplites);
        _startupPlacementCounter = 0;
        _currentToken = null;
        OnPlacementStarted?.Invoke();
    }
    private void OnHeroBonusCompleted() {
        Debug.Log("Hero starting bonus completed!");
        OnPlacementCompleted?.Invoke();        
    }

    public bool StartPlacingToken(TokenType tokenType) {
        if (_tokenHolder.HasObject()) {
            _tokenHolder.DestroyTokenView();
        }
        if (!_tracker.CanPlace(tokenType)) {
            Debug.Log($"[TokenPlacementManager] Cannot start placing {tokenType} — limit reached");
            return false;
        }
        if (tokenType is TokenType.Hero) {
            _currentToken = GameState.Instance.CurrentPlayer.Hero;
        } else if (tokenType is TokenType.Hoplite) {
            _currentToken = _tokenModelFactory.CreateHoplite(GameState.Instance.CurrentPlayer);
        }
        TokenView tokenView = _tokenViewFactory.CreateTokenView(_currentToken);

        _tokenHolder.AttachToken(tokenView);

        float radius = _tokenViewFactory.GetRadius(tokenType);
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
    private void PlaceToken(RegionId regionId, Player currentPlayer) {
        var tokenType = _currentToken.Type;
        _tracker.CountToken(tokenType);
        
        var position = _tokenHolder.GetGameObjectPosition();
        if (position.HasValue) {
            _recorder.AddStep(currentPlayer.Color, tokenType, regionId, position.Value);
        }
        _startupPlacementCounter++;

        TokenView token = null;
        if (tokenType == TokenType.Hoplite && _regionManager.IsHopliteInRegion(currentPlayer.Color, regionId)) {
            var hopliteGo = _regionVisuals.GetHopliteFromRegion(regionId, currentPlayer.Color);
            _tokenHolder.DestroyTokenView();
            token = _tokenHolder.UnattachToken();
        } else {
            SpawnPoint spawnPoint = _regionVisuals.PlaceToken(_tokenHolder.TokenView.gameObject, regionId, _tokenHolder.GetGameObjectPosition());
            if (spawnPoint != null) {
                _tokenVisualChanger.PrepareTokenPlacement(_tokenHolder.TokenView.gameObject, currentPlayer.Color);
                _tokenHolder.SetGameObjectPosition(spawnPoint.Position);
                token = _tokenHolder.UnattachToken();
            }
        }
        _regionManager.RegisterToken(regionId, _currentToken);
        _currentToken = null;
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
        _tracker.Reset();
    } 
}
