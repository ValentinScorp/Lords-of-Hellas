using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupData
{
    [SerializeField] private PlayerSetupConfigList _playerSetupConfigList;
    public PlayerSetupConfigList Players => _playerSetupConfigList;

    private const int MinPlayers = 2;
    private const int MaxPlayers = 4;

    public bool StartGame(List<PlayerSetupPanel> playerPanels) {
        if (playerPanels.Count < MinPlayers) {
            Debug.LogWarning("Недостатньо гравців!");
            return false;
        }
        _playerSetupConfigList = ScriptableObject.CreateInstance<PlayerSetupConfigList>();
        _playerSetupConfigList.Players.Clear();
        for (int i = playerPanels.Count - 1; i >= 0; i--) {
            _playerSetupConfigList.Players.Add(playerPanels[i].GetConfig());
        }
        //GameContext.Instance.SetPlayers(_playerSetupConfigList.Players);

        return true;
    }

    public bool ValidatePlayers(List<PlayerSetupPanel> playerPanels) {
        bool isValid = true;

        var names = new HashSet<string>();
        var colors = new HashSet<PlayerColor>();
        var heroes = new HashSet<Hero.Id>();

        foreach (var panel in playerPanels) {
            var config = panel.GetConfig();
            if (string.IsNullOrWhiteSpace(config.PlayerName)) {
                isValid = false;
                break;
            }
            if (!names.Add(config.PlayerName)) {
                isValid = false;
                break;
            }
            if (!colors.Add(config.PlayerColor)) {
                isValid = false;
                break;
            }
            if (!heroes.Add(config.HeroId)) {
                isValid = false;
                break;
            }
        }
        if (playerPanels.Count < MinPlayers) {
            isValid = false;
        }

        return isValid;
    }

}
