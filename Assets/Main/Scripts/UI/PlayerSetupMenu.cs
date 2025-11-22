using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenu : MonoBehaviour
{
    [SerializeField] private Transform _playerListContainer;
    [SerializeField] private GameObject _playerPanelPrefab;
    [SerializeField] private Button _addPlayerButton;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private PlayerColorPalette _playerColorPalette;
    
    private PlayerSetupData _playerSetupData = new();
    private List<PlayerSetupPanel> _playerPanels = new();
    private const int MaxPlayers = 4;

    private void Start() {
        _addPlayerButton.onClick.AddListener(() => AddPlayerPanel());
        _startGameButton.onClick.AddListener(StartGame);

        AddPlayerPanel("Alice", "Achilles", PlayerColor.Red);
        AddPlayerPanel("Bob", "Heracles", PlayerColor.Blue);
    }

    private void AddPlayerPanel(string playerName = null, string heroName = null, PlayerColor? color = null) {
        if (_playerPanels.Count >= MaxPlayers) {
            Debug.LogWarning("Can't add more players!");
            return;
        }
        GameObject panelGO = Instantiate(_playerPanelPrefab, _playerListContainer);
        var panel = panelGO.GetComponent<PlayerSetupPanel>();
        var availableColors = _playerColorPalette.GetAvailablePlayerColors();

        var heroes = new List<HeroSetup>
         {
            new(Hero.Id.Achilles),
            new(Hero.Id.Heracles),
            new(Hero.Id.Perseus),
            new(Hero.Id.Helen)
        };

        panel.Init(heroes, availableColors);
        panel.SetPlayerName(playerName);
        panel.SetHero(heroName);
        if (color.HasValue) {
            panel.SetColor(color.Value);
        }

        panel.OnConfigChanged += CheckStartGameButtonInteractable;

        _playerPanels.Add(panel);
        CheckStartGameButtonInteractable();
    }

    private void StartGame() {
        if (_playerSetupData.StartGame(_playerPanels)) {
            SceneManager.LoadScene("10_GameScene");
        }        
    }
    private void CheckStartGameButtonInteractable() {
        _startGameButton.interactable = _playerSetupData.ValidatePlayers(_playerPanels);
    }
}
