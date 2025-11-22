using UnityEngine;
using UnityEngine.UI;

public class RegionBorderEmissionController : MonoBehaviour
{
    public Color baseEmissionColor = Color.white;
    public float pulseSpeed = 5f;
    public float minIntensity = 0f;
    public float maxIntensity = 2f;

    private Renderer _renderer;
    private bool _isPulsing = false;
    private bool _isIntensityMax = false;
    private float _intensity;
    private MaterialPropertyBlock _propertyBlock;
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    private void Awake() {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null) {
            Debug.LogError("Error getting Renderer in EmissionPulseController!", gameObject);
        }
    }

    private void Start() {
        _propertyBlock = new MaterialPropertyBlock();

        SetEmissionColor(baseEmissionColor * minIntensity);
    }

    private void Update() {
        if (_isPulsing) {
            _intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed /*+ Mathf.PI / 2f*/) + 1f) * 0.5f);
        } else {
            if (_isIntensityMax) {
                _intensity = maxIntensity;
            } else {
                _intensity = minIntensity;
            }
        }
        SetEmissionColor(baseEmissionColor * _intensity);
    }

    public void EnablePulse() {
        _isPulsing = true;
    }

    public void DisablePulse() {
        _isPulsing = false;
        _isIntensityMax = false;

        SetEmissionColor(baseEmissionColor * minIntensity);
    }

    public void SetEmissionMax() {
        _isPulsing = false;
        _isIntensityMax = true;
    }

    public void SetEmissionMin() {
        _isPulsing = false;
        _isIntensityMax = false;
    }

    private void SetEmissionColor(Color color) {
        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetColor(EmissionColorID, color);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
