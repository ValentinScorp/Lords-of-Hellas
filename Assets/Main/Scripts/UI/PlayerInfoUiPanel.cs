using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUiPanel : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TMP_Text _currentGamePhaseText;
    [SerializeField] private CardArtifactIconPanel _artifactIconPanel;
    [SerializeField] private CardCombatIconPanel _combatIconPanel;
    [SerializeField] private TMP_Text _heroNameText;
    [SerializeField] private TMP_Text _hoplitesInfoText;

    private Player _currentPlayer;
    private static Color _defaultBackgroundColor = Color.white;
    private void Awake()
    {
        SceneUiRegistry.Register(this);
    }
    private void OnDestroy()
    {
        if (_currentPlayer is not null) {
            _currentPlayer.OnPlayerInfoChange -= UpdatePlayerInfo;
        }
        _currentPlayer = null;
        SceneUiRegistry.Unregister<PlayerInfoUiPanel>();
    }
    private void Start()
    {
        SetBackgroundColor(_defaultBackgroundColor);
    }
    // private void OnEnable()
    // {
    //     GameContext.Instance.OnPlayerChanged += HandlePlayerChanged;
    // }
    // private void OnDisable()
    // {
    //     GameContext.Instance.OnPlayerChanged -= HandlePlayerChanged;
    // }
    public void BindPlayer(Player newPlayer)
    {
        SetBackgroundColor(newPlayer.Color);

        if (_currentPlayer != null) {
            _currentPlayer.OnPlayerInfoChange -= UpdatePlayerInfo;
        }
        _currentPlayer = newPlayer;
        if (_currentPlayer != null) {
            _currentPlayer.OnPlayerInfoChange += UpdatePlayerInfo;
        }
        UpdatePlayerInfo(newPlayer);
    }

    private void UpdatePlayerInfo(Player player)
    {
        Debug.Log("Updating player info!");
        _heroNameText.text = player.Hero.DisplayName;
        _hoplitesInfoText.text = $"Total {player.HoplitesOnBoard} hoplites on board!";
        _combatIconPanel.ClearPanel();
        foreach (var combatCard in player.CombatCards) {
            _combatIconPanel.AddCardIcon(combatCard);
        }
        _artifactIconPanel.ClearPanel();
        foreach (var artifact in player.ArtifactCards) {
            _artifactIconPanel.AddCardIcon(artifact);
        }
    }
    private void SetBackgroundColor(PlayerColor color)
    {
        var palette = GameContent.PlayerColorPalette;
        switch (color) {
            case PlayerColor.Red: SetBackgroundColor(palette.Red); break;
            case PlayerColor.Green: SetBackgroundColor(palette.Green); break;
            case PlayerColor.Blue: SetBackgroundColor(palette.Blue); break;
            case PlayerColor.Yellow: SetBackgroundColor(palette.Yellow); break;
            default: SetBackgroundColor(_defaultBackgroundColor); break;
        }
    }
    private void SetBackgroundColor(Color color)
    {
        if (_backgroundImage != null) {
            _backgroundImage.color = new Color(color.r, color.g, color.b, _backgroundImage.color.a);
        } else {
            Debug.LogError("Image color not assigned!");
        }
    }
    private void HandleGamePhaseChanged(GamePhaseBase gamePhase)
    {
        _currentGamePhaseText.text = gamePhase.Name;
    }
}
