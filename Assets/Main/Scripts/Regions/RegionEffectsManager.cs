using UnityEngine;

public class RegionEffectsManager : MonoBehaviour
{
    [SerializeField] private RegionId _regionId;
    [SerializeField] private Renderer _regionBorder;
    [SerializeField] private RegionInfoUiPanel _regionInfoUiIPanel;

    private RegionBorderEmissionController _emissionController;
    private bool _isSelected = false;
    private Material _uniqueMaterial;

    public RegionId RegionId => _regionId;

    private void Awake() {
        if (_regionBorder == null) {
            Debug.LogError("Error getting target object in RegionSelectManager!", gameObject);
            return;
        }
        _emissionController = _regionBorder.GetComponent<RegionBorderEmissionController>();
        if (_emissionController == null) {
            Debug.LogError("Error getting EmissionPulseController component of target object in RegionSelectManager!", gameObject);
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) {
            Debug.Log("Renderer not found!");
        } else { 
            Material baseMaterial = renderer.sharedMaterial;
            _uniqueMaterial = Material.Instantiate(baseMaterial);
            renderer.material = _uniqueMaterial;
            UnhighlightRegion();
        }
    }
    private void OnDestroy() {
        if (_uniqueMaterial != null) {
            Destroy(_uniqueMaterial);
        }
    }

    public void SubscribeOnOwnerChanged(RegionDataRuntime regionData) {
        regionData.OnOwnerChanged += HandleOwnerChanged;
        HandleOwnerChanged(regionData.OwnedBy);
    }

    public void HandleOwnerChanged(PlayerColor newOwner) {
        var palette = GameData.PlayerColorPalette;
        Color color = palette.Grey;

        switch (newOwner) {
            case PlayerColor.Red: color = palette.Red; break;
            case PlayerColor.Blue: color = palette.Blue; break;
            case PlayerColor.Green: color = palette.Green; break;
            case PlayerColor.Yellow: color = palette.Yellow; break;
            case PlayerColor.Purple: color = palette.Purple; break;
            case PlayerColor.Brown: color = palette.Brown; break;
        }

        if (newOwner == PlayerColor.Gray) {
            UnhighlightRegion();
        } else {
            HighlightRegion();
        }
        SetColor(color);
    }
    private void OnMouseEnter() {
        if (!_isSelected) { 
            _emissionController?.EnablePulse();

            //HighlightRegion();
        }
    }
    private void OnMouseExit() {
        if (!_isSelected) {
            _emissionController?.DisablePulse();

            //UnhighlightRegion();
        }
    }

    public void Activate() {
        _isSelected = true;
        _emissionController?.SetEmissionMax();
    }
    public void Deactivate() {
        _isSelected = false;
        _emissionController?.SetEmissionMin();
    }

    private void HighlightRegion() {
        SetColorAlpha(0.2f);

        //SetColor(GameContext.PlayerColorPalette.Red);
    }
    private void UnhighlightRegion() {
        SetColorAlpha(0.0f);
    }

    private void SetColorAlpha(float alpha) {
        if (_uniqueMaterial == null) {
            return;
        }
        Color color = _uniqueMaterial.color;
        color.a = alpha;
        _uniqueMaterial.color = color;
    }
    private void SetColor(Color color) {
        if (_uniqueMaterial == null) {
            return;
        }
        Color old = _uniqueMaterial.color;
        _uniqueMaterial.color = new Color(color.r, color.g, color.b, old.a);
    }
}
