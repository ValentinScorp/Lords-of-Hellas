using System.Collections.Generic;
using UnityEngine;

public class LandTokenUiPanel : MonoBehaviour
{
    [SerializeField] private LandTokenIcon _iconPrefab;
    private readonly List<LandTokenIcon> _icons = new();
    private Player _player;

    private void Start()
    {
        GameContext.Instance.LandTokens.TokenOwnerChanged += OnTokenOwnerChanged;
    }

    private void OnDestroy()
    {
        GameContext.Instance.LandTokens.TokenOwnerChanged -= OnTokenOwnerChanged;
    }

    internal void Bind(Player player)
    {
        Unbind();

        if (player is null) return;

        _player = player;
        
        RefreshView(_player);
    }

    internal void Unbind()
    {        
        _player = null;
        ClearIcons();
    }

    private void OnTokenOwnerChanged(LandToken token, PlayerColor oldOwner)
    {
        if (_player is null) return;

        if (token.OwnerColor == _player.Color || oldOwner == _player.Color) {
            RefreshView(_player);
        }
    }

    private void RefreshView(Player player)
    {
        ClearIcons();

        foreach(var landId in player.LandTokens) {
            AddIcon(landId);
        }
    }

    private void AddIcon(LandId landId)
    {
        if (_icons is null || _iconPrefab is null) return;

        var icon = Instantiate(_iconPrefab, gameObject.transform, false);
        icon.SetColorLand(landId);
        _icons.Add(icon);
    }
    private void ClearIcons()
    {
        if (_icons is null) return;

        foreach(var icon in _icons) {
            Destroy(icon.gameObject);
        }
        _icons.Clear();
    }

    internal void Show(bool activate)
    {
        gameObject.SetActive(activate);
    }
}
