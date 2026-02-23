using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TokenView : MonoBehaviour, ISelectable
{
    public static event Action<TokenView, PointerEventData> Clicked;
    [SerializeField] private TMP_Text _leadershipText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _strengthText;

    private static readonly int FresnelTintId = Shader.PropertyToID("_FresnelTint");
    const float GHOST_COLOR_INTENCITY = 5.0f;

    private MaterialPropertyBlock _mpb;

    private Renderer _renderer = null;
    private Transform _canvas = null;
    private TextMeshProUGUI _label = null;
    private TokenViewModel _viewModel;
    public TokenType TokenType => _viewModel != null ? _viewModel.Model.Type : TokenType.None;   
    public PlayerColor PlayerColor => _viewModel != null ? _viewModel.Model.PlayerColor : PlayerColor.Gray;
    public RegionId RegionId => _viewModel != null ? _viewModel.RegionId : RegionId.Unknown;
    public RegionNest Nest => _viewModel != null ? _viewModel.TokenNest : null;
    
    public TokenViewModel ViewModel => _viewModel;

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();

        _renderer = transform.Find("MeshVisual")?.GetComponent<Renderer>();
        _canvas = transform.Find("Canvas");
        _label = _canvas?.GetComponentInChildren<TextMeshProUGUI>(true);

        if (!_renderer || !_canvas || !_label) {
            Debug.LogError($"Initialization failed in {nameof(TokenView)}: Missing components.");
            enabled = false;
        }
    }
    public void HandleLeadershipChanged(int value)
    {
        _leadershipText.text = value.ToString();
    }
    public void HandleSpeedChanged(int value)
    {
        _speedText.text = value.ToString();
    }
    public void HandleStrengthChanged(int value)
    {
        _strengthText.text = value.ToString();
    }
    public void SetLayer(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0) {
            Debug.LogError($"Layer '{layerName}' not found.");
            return;
        }
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public void SetTag(string tagName)
    {
        gameObject.tag = tagName;
    }
    public void SetGhostMaterial()
    {
        if (_renderer != null) {
            _renderer.material = GameContent.TokenMaterialPalette.ghostMaterial;
        }
    }
    public void SetGhostColor(TerrainValidator.GhostState ghostState)
    {
        switch (ghostState) {
            case TerrainValidator.GhostState.Neutral:
                SetGhostColor(GameContent.TokenMaterialPalette.ghostColorInit);
                break;
            case TerrainValidator.GhostState.Allowed:
                SetGhostColor(GameContent.TokenMaterialPalette.ghostColorOk);
                break;
            case TerrainValidator.GhostState.Forbidden:
                SetGhostColor(GameContent.TokenMaterialPalette.ghostColorError);
                break;
            default:
                SetGhostColor(GameContent.TokenMaterialPalette.ghostColorInit);
                break;
        }
    }
    public void SetGhostColor(Color color)
    {
        if (_renderer == null || _renderer.material == null) {
            Debug.LogError("TokenView: Renderer or material is missing!");
            return;
        }
        _renderer.material.SetColor(FresnelTintId, color * GHOST_COLOR_INTENCITY);
    }
    public void SetGhostColorMPB(Color color)
    {
        if (_renderer == null) return;

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(FresnelTintId, color * GHOST_COLOR_INTENCITY);
        _renderer.SetPropertyBlock(_mpb);
    }
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    public void AdjustPositionToSpawnPoint()
    {
        if (Nest != null) {
            SetPosition(Nest.Position);
        } else {
            Debug.LogWarning("SpawnPoint doesn't set in TokenView!");
        }
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void ChangeToPlayerMaterial()
    {
        var renderer = gameObject.GetComponentInChildren<Renderer>();
        if (renderer == null) {
            Debug.LogWarning("No renderer found!");
            return;
        }
        renderer.material = GameContent.TokenMaterialPalette.GetPlayerMaterial(PlayerColor);
    }
    public void SetLabel(string text)
    {
        if (_label == null) {
            Debug.LogWarning("TextMeshProUGUI not found.");
            return;
        }
        _label.text = text;
    }

    public void SetCount(int count)
    {
        if (count <= 0) {
            Debug.LogWarning("Count must be positive.");
            return;
        }
        SetLabel(count.ToString());
    }

    public int GetHopliteCount()
    {
        if (_label == null || !int.TryParse(_label.text, out int value) || value <= 0) {
            return -1;
        }
        return value;
    }
    public void OnClick(Vector3 hitPoint)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) {
            position = Camera.main.WorldToScreenPoint(hitPoint)
        };
        Clicked?.Invoke(this, eventData);
    }
    public void SubscribeToViewModel(TokenViewModel viewModel)
    {
        _viewModel = viewModel;

        if (_viewModel is HeroViewModel heroVm) {
            heroVm.LeadershipChanged += HandleLeadershipChanged;
            heroVm.SpeedChanged += HandleSpeedChanged;
            heroVm.StrengthChanged += HandleStrengthChanged;
            heroVm.RefreshStats();
            SetLabel(heroVm.DisplayName);
        }

        if (_viewModel is HopliteStackViewModel hopliteVm) {
            hopliteVm.CountChanged += SetCount;
            hopliteVm.RefreshCount();
        }
        _viewModel.WorldPositionChanged += SetPosition;
        _viewModel.VisualStateChanged += HandleVisualState;
    }

    private void UnsubscribeFromViewModel()
    {
        if (_viewModel is HeroViewModel heroVm) {
            heroVm.LeadershipChanged -= HandleLeadershipChanged;
            heroVm.SpeedChanged -= HandleSpeedChanged;
            heroVm.StrengthChanged -= HandleStrengthChanged;
        }

        if (_viewModel is HopliteStackViewModel hopliteVm) {
            hopliteVm.CountChanged -= SetCount;
        }
        _viewModel.WorldPositionChanged -= SetPosition;
        _viewModel.VisualStateChanged -= HandleVisualState;
    }
    private void HandleVisualState(TokenViewModel.VisualState state, PlayerColor playerColor)
    {
        if (state == TokenViewModel.VisualState.Placed) {
            SetLayer("HoplonToken");
            SetTag("PlacedToken");
            ChangeMaterial(playerColor);
        }
    }
    private void ChangeMaterial(PlayerColor playerColor)
    {
        if (GameContent.Instance.TryGetPlayerMaterial(playerColor, out var material)) {
            _renderer.material = material;
        }
    }
    private void OnDestroy()
    {
        ServiceLocator.Get<TokenViewRegistry>()?.Unregister(_viewModel, this);
        UnsubscribeFromViewModel();
        _viewModel.Dispose();
    }
}
