using UnityEngine;

public class PlayerColorable : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    private readonly int _playerColorPropertyId = Shader.PropertyToID("_PlayerColor");
    
    private const float _PlayerColorIntensity = 1.0f;

    private MaterialPropertyBlock _mpb;

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();
    }
    public void SetPlayerColor(PlayerColor playerColor)
    {
        var color = GameContent.Instance.GetPlayerColor(playerColor);
        SetColor(color);
    }
    private void SetColor(Color color)
    {
        if (_renderer is null) return;

        _renderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(_playerColorPropertyId, color * _PlayerColorIntensity);
        _renderer.SetPropertyBlock(_mpb);
    }
}

