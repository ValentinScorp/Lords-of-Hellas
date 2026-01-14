using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TokenView : MonoBehaviour, ISelectable
{
    public static event Action<TokenView, PointerEventData> Clicked;
    [SerializeField] private TokenType _tokenType;
    [SerializeField] private TokenModel _model;
    [SerializeField] private TMP_Text _leadershipText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _strengthText;

    private static readonly int FresnelTintId = Shader.PropertyToID("_FresnelTint");
    const float GHOST_COLOR_INTENCITY = 5.0f;

    private Renderer _renderer = null;
    private Transform _canvas = null;
    private TextMeshProUGUI _label = null;
    public TokenType TokenType => _tokenType;
    // public TokenModel Model => _model;
    public PlayerColor PlayerColor { get; set; }
    public RegionId RegionId => _viewModel != null ? _viewModel.RegionId : RegionId.Unknown;
    public SpawnPoint SpawnPoint { get; set; }
    private TokenViewModel _viewModel;
    public TokenViewModel ViewModel => _viewModel;

    private void Awake()
    {
        _renderer = transform.Find("MeshVisual")?.GetComponent<Renderer>();
        _canvas = transform.Find("Canvas");
        _label = _canvas?.GetComponentInChildren<TextMeshProUGUI>(true);

        if (!_renderer || !_canvas || !_label) {
            Debug.LogError($"Initialization failed in {nameof(TokenView)}: Missing components.");
            enabled = false;
        }
    }
    public void SubscribeOnModel(TokenModel model)
    {
        if (model.Type != TokenType) {
            Debug.LogWarning($"Can't subscribe TokenView type {TokenType} on model type {model.Type}");
            return;
        }
        _model = model;
        _viewModel = CreateViewModel(model);
        
        if (_viewModel is HopliteStackViewModel hopliteStackVm) {
            hopliteStackVm.CountChanged += count => SetLabel(count.ToString());
        }
        if (_viewModel is HeroViewModel heroVm) {
            SetLabel(heroVm.DisplayName);
            heroVm.LeadershipChanged += value => HandleLeadershipChanged(value);
            heroVm.SpeedChanged += value => HandleSpeedChanged(value);
            heroVm.StrengthChanged += value => HandleStrengthChanged(value);
            heroVm.RefreshStats();
        }
    }
    private TokenViewModel CreateViewModel(TokenModel model)
    {
        return model switch
        {
            HeroModel hero => new HeroViewModel(hero),
            HopliteStackModel hoplite => new HopliteStackViewModel(hoplite),
            _ => new TokenViewModel(model),
        };
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
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    public void AdjustPositionToSpawnPoint()
    {
        if (SpawnPoint != null) {
            SetPosition(SpawnPoint.Position);
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
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Camera.main.WorldToScreenPoint(hitPoint)
        };
        Clicked?.Invoke(this, eventData);
    }
    private void SubscribeToViewModel()
    {
        if (_viewModel is HeroViewModel heroVm) {
            heroVm.LeadershipChanged += HandleLeadershipChanged;
            heroVm.SpeedChanged += HandleSpeedChanged;
            heroVm.StrengthChanged += HandleStrengthChanged;
        }

        if (_viewModel is HopliteStackViewModel hopliteVm) {
            hopliteVm.CountChanged += SetCount;
        }
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
    }
    private void OnDestroy()
    {
        UnsubscribeFromViewModel();
        _viewModel.Dispose();
    }
}
