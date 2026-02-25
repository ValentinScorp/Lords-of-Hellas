using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class TokenView : MonoBehaviour, IHittable, IPlaceableVisual
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
    private TokenModel _model;
    private RegionNest _nest;

    public TokenModel Model => _model;
    public RegionNest Nest => _nest;
    public TokenType TokenType => _model != null ? _model.Type : TokenType.None;   
    public PlayerColor PlayerColor => _model != null ? _model.PlayerColor : PlayerColor.Gray;
    public RegionId RegionId => _model != null ? _model.RegionId : RegionId.Unknown;
    public Vector3 WorldPosition => transform.position;
    
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
    public void SubscribeToModel(TokenModel model)
    {
        if (model == null) {
            Debug.Log($"Unable to subscribe to model {model}. Model is null!");
            return;
        }

        _model = model;

        _model.RegionChanged += RegionChanged;
        
        if (_model is HeroModel hero) {
            hero.LeadershipChanged += OnLeadershipChanged;
            hero.SpeedChanged += OnSpeedChanged;
            hero.StrengthChanged += OnStrengthChanged;
            hero.RefreshStats();
            SetLabel(hero.DisplayName);
        }

        if (_model is HopliteStackModel hoplite) {
            hoplite.CountChanged += SetCount;
            hoplite.RefreshCount();
        }
    }    

    private void UnsubscribeFromModel()
    {
        if (_model is null) return;

        _model.RegionChanged -= RegionChanged;

        if (_model is HeroModel hero) {
            hero.LeadershipChanged -= OnLeadershipChanged;
            hero.SpeedChanged -= OnSpeedChanged;
            hero.StrengthChanged -= OnStrengthChanged;
        }

        if (_model is HopliteStackModel hoplite) {
            hoplite.CountChanged -= SetCount;
        }
        _model = null;
    }
    private void RegionChanged(TokenModel model)
    {
        var regions = ServiceLocator.Get<RegionsView>();
        if (regions == null) return;
        if (regions.TryGetNest(model.RegionId, model.NestId, out var nest)) {
            _nest = nest;
            AdjustPositionToNest();
            SetLayer("HoplonToken");
            SetTag("PlacedToken");
            ChangeMaterial(PlayerColor);
        }
    }
    public void OnLeadershipChanged(int value)
    {
        _leadershipText.text = value.ToString();
    }
    public void OnSpeedChanged(int value)
    {
        _speedText.text = value.ToString();
    }
    public void OnStrengthChanged(int value)
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

    public void AdjustPositionToNest()
    {
        if (Nest != null) {
            SetWorldPosition(Nest.Position);
        } else {
            Debug.LogWarning("SpawnPoint doesn't set in TokenView!");
        }
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
    private void ChangeMaterial(PlayerColor playerColor)
    {
        if (GameContent.Instance.TryGetPlayerMaterial(playerColor, out var material)) {
            _renderer.material = material;
        }
    }
    private void OnDestroy()
    {
        UnsubscribeFromModel();
    }

    internal void SetWorldPosition(Vector3 position)
    {
        transform.position = position;
    }
}
