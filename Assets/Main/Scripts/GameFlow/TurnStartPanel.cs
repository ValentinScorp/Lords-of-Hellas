using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class TurnStartPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _title;
    [SerializeField] Button _okButton;

    private Action <Player> _Confirmed;
    private Player _player;


    private const string UiTable = "UI";

    private static readonly Dictionary<PlayerColor, string> PlayerColorKey = new() {
        { PlayerColor.Red, "player.color.red.genitive" },
        { PlayerColor.Blue, "player.color.blue.genitive" },
        { PlayerColor.Green, "player.color.green.genitive" },
        { PlayerColor.Yellow, "player.color.yellow.genitive" },
    };
    
    private void Awake()
    {
        Show(false);
        SceneUiRegistry.Register(this);
        _okButton.onClick.AddListener(HandleOkButton);
    }
    private void OnDestroy()
    {
        _okButton.onClick.RemoveListener(HandleOkButton);
        SceneUiRegistry.Unregister<TurnStartPanel>();
    }
    public void Launch(Player player, Action<Player> Confirmed)
    {
        _player = player;
        _Confirmed = Confirmed;

        Color color = GameContent.Instance.GetPlayerColor(player.Color);
        var hexColor = ColorUtility.ToHtmlStringRGB(color); 

        var colorWord = LocalizationSettings.StringDatabase.GetLocalizedString(
            UiTable, PlayerColorKey[player.Color]);
            
        _title.text = LocalizationSettings.StringDatabase.GetLocalizedString(
            UiTable,
            "turn.player.colored",
            new object[] { hexColor, colorWord });

        Show(true);
    }
    public void HandleOkButton()
    {
        Show(false);
        _Confirmed.Invoke(_player);
        _Confirmed = null;
    }
    public void Show(bool show) {
        gameObject.SetActive(show);
    }
}
