using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class PlayerSetupPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private TMP_Dropdown _heroDropdown;
    [SerializeField] private TMP_Dropdown _colorDropdown;

    private List<HeroSetup> _heroes;
    private List<PlayerColor> _colors;

    public event Action OnConfigChanged;

    public void Init(List<HeroSetup> heroes, List<PlayerColor> colors) {
        _heroes = heroes?.ToList() ?? throw new ArgumentNullException(nameof(heroes));
        _colors = colors?.ToList() ?? throw new ArgumentNullException(nameof(colors));

        SetupDropdown(_heroDropdown, _heroes.Select(h => h.DisplayName));
        SetupDropdown(_colorDropdown, _colors.Select(c => c.ToString()));

        _nameField.onValueChanged.AddListener(_ => NotifyChanged());
        _heroDropdown.onValueChanged.AddListener(_ => NotifyChanged());
        _colorDropdown.onValueChanged.AddListener(_ => NotifyChanged());
    }
    private static void SetupDropdown(TMP_Dropdown dropdown, IEnumerable<string> options) {
        dropdown.ClearOptions();
        dropdown.AddOptions(options.ToList());
    }
    public void SetPlayerName(string name) => _nameField.text = name;
    public void SetHero(string heroName) {
        int index = _heroDropdown.options.FindIndex(o => o.text == heroName);
        if (index >= 0) _heroDropdown.value = index;
    }
    public void SetColor(PlayerColor color) {
        int index = _colors.IndexOf(color);
        if (index >= 0) _colorDropdown.value = index;
    }
    public PlayerSetupConfig GetConfig() {
        HeroModel.Id heroId = _heroes[_heroDropdown.value].HeroId;
        PlayerColor color = _colors[_colorDropdown.value];

        return new PlayerSetupConfig {
            PlayerName = _nameField.text,
            HeroId = heroId,
            PlayerColor = color
        };
    }
    private void NotifyChanged() => OnConfigChanged?.Invoke();
}
