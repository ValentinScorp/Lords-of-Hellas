using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Profiling.HierarchyFrameDataView;

public class TokenView : MonoBehaviour
{
    [SerializeField] private TokenType _tokenType;

    [SerializeField] private TMP_Text _leadershipText;
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private TMP_Text _strengthText;

    private static readonly int FresnelTintId = Shader.PropertyToID("_FresnelTint");
    const float GHOST_COLOR_INTENCITY = 5.0f;

    private Renderer _renderer = null;
    private Transform _visual = null;
    private Transform _canvas = null;
    private TextMeshProUGUI _label = null;

    public TokenType TokenType => _tokenType;
    public PlayerColor PlayerColor { get; set; }
    public RegionId RegionId { get; set; }
    public int SpawnPointId { get; set; }

    private void Awake() {
        _visual = transform.Find("Visual");
        _renderer = _visual?.GetComponent<Renderer>();
        _canvas = transform.Find("Canvas");
        _label = _canvas?.GetComponentInChildren<TextMeshProUGUI>(true);

        if (!_visual || !_renderer || !_canvas || !_label) {
            Debug.LogError($"Initialization failed in {nameof(TokenView)}: Missing components.");
            enabled = false;
        }
    }
    public void SubscribeOnModel(Token model) {
        if (model is HopliteStack hoplite) {
            hoplite.OnCountChanged += count => SetLabel(count.ToString());
        }
        if (model is Hero hero) {
            Debug.Log("Subscribing on hero!");
            SetLabel(hero.DisplayName);
            hero.OnLeadershpChanged += value => _leadershipText.text = value.ToString();
            hero.OnSpeedChanged += value => _speedText.text = value.ToString();
            hero.OnStrengthChanged += value => _strengthText.text = value.ToString();
            hero.ChangeStrength(0);
            hero.ChangeSpeed(0);
            hero.ChangeLeadership(0);
        }
    }
    public void SetVisualLayer(string layerName) {
        if (_visual == null) return;

        int layer = LayerMask.NameToLayer(layerName);
        if (layer < 0) {
            Debug.LogError($"Layer '{layerName}' not found.");
            return;
        }
        _visual.gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public void SetVisualTag(string tagName) {
        if (_visual == null) {
            return;
        }
#if UNITY_EDITOR
        if (!UnityEditorInternal.InternalEditorUtility.tags.Contains(tagName)) {
            Debug.LogWarning($"Tag '{tagName}' does not exist.");
            return;
        }
#endif
        _visual.tag = tagName;
    }
    public void SetGhostMaterial() {
        if (_renderer != null) {
            //_renderer.material = GameContext.TokenMaterialPalette.ghostMaterial;
        }
    }
    public void SetGhostColor(TokenPlacementTerrainValidator.GhostState ghostState) {
        switch (ghostState) {
            case TokenPlacementTerrainValidator.GhostState.Neutral:
                //SetGhostColor(GameContext.TokenMaterialPalette.ghostColorInit);
                break;
            case TokenPlacementTerrainValidator.GhostState.Allowed:
                //SetGhostColor(GameContext.TokenMaterialPalette.ghostColorOk);
                break;
            case TokenPlacementTerrainValidator.GhostState.Forbidden:
                //SetGhostColor(GameContext.TokenMaterialPalette.ghostColorError);
                break;
            default:
                //SetGhostColor(GameContext.TokenMaterialPalette.ghostColorInit);
                break;
        }
    }
    public void SetGhostColor(Color color) {
        if (_renderer == null || _renderer.material == null) {
            Debug.LogError("TokenView: Renderer or material is missing!");
            return;
        }

        //var oldColor = _renderer.material.GetColor(FresnelTintId);
        
        _renderer.material.SetColor(FresnelTintId, color * GHOST_COLOR_INTENCITY);
        //Debug.Log($"Fresnel color : {oldColor}");

        //if (_renderer.material.GetColor(FresnelTintId) != color) {
        //    Debug.LogError($"Failed to set _FresnelTint! Was: {oldColor}, Set: {color}");
        //} else {
        //    Debug.Log($"Fresnel color set: {color}");
        //}
    }
    public void SetLabel(string text) {
        if (_label == null) {
            Debug.LogWarning("TextMeshProUGUI not found.");
            return;
        }
        _label.text = text;
    }

    public void SetCount(int count) {
        if (count <= 0) {
            Debug.LogWarning("Count must be positive.");
            return;
        }
        SetLabel(count.ToString());
    }

    public int GetHopliteCount() {
        if (_label == null || !int.TryParse(_label.text, out int value) || value <= 0) {
            return -1;
        }
        return value;
    }
}
