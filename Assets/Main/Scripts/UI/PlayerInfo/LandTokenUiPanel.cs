using System.Collections.Generic;
using UnityEngine;

public class LandTokenUiPanel : MonoBehaviour
{
    [SerializeField] private LandTokenIcon _iconPrefab;
    private const int _MaxIcons = 5;
    private readonly List<LandTokenIcon> _icons = new();
    private Player _player;
    private LandOwnershipWatcher _manager;

    private void Awake()
    {
        InstantiateIcons(_MaxIcons);
    }

    private void OnDestroy()
    {
        Unbind();
    }

    internal void Bind(Player player, LandOwnershipWatcher manager)
    {
        Unbind();

        if (player is null || manager is null) {
            return;
        }

        _player = player;
        _manager = manager;
        _manager.TokenOwnerChanged += OnTokenOwnerChanged;
        Refresh();
    }

    internal void Unbind()
    {
        if (_manager is not null) {
            _manager.TokenOwnerChanged -= OnTokenOwnerChanged;
            _manager = null;
        }

        _player = null;
        GreyoutAll();
    }

    private void OnTokenOwnerChanged(LandToken token, PlayerColor previousOwner, PlayerColor newOwner)
    {
        if (_player is null) {
            return;
        }

        if (previousOwner == _player.Color || newOwner == _player.Color) {
            Refresh();
        }
    }

    private void Refresh()
    {
        if (_player is null || _manager is null) {
            GreyoutAll();
            return;
        }

        var playerTokens = _manager.GetOwnedLandTokens(_player.Color);
        for (int i = 0; i < _icons.Count; i++) {
            if (i < playerTokens.Count) {
                _icons[i].SetColorLand(playerTokens[i]);
            } else {
                _icons[i].Greyout();
            }
        }
    }

    private void GreyoutAll()
    {
        for (int i = 0; i < _icons.Count; i++) {
            _icons[i].Greyout();
        }
    }

    private void InstantiateIcons(int count)
    {
        for (int i = 0; i < count; i++) {
            var icon = Instantiate(_iconPrefab, gameObject.transform, false);
            icon.Greyout();
            _icons.Add(icon);
        }
    }

    internal void Show(bool activate)
    {
        gameObject.SetActive(activate);
    }
}
