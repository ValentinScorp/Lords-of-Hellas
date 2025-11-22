using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TMP_Text _currentGamePhaseText;
    [SerializeField] private CardArtifactIconPanel _artifactIconPanel;
    [SerializeField] private CardCombatIconPanel _combatIconPanel;

    private Player _currentPlayer;
    private static Color _defaultBackgroundColor = Color.white;
    private void Start() {
        SetBackgroundColor(_defaultBackgroundColor);
        
    }
    private void OnEnable() {
        GameState.Instance.OnPlayerChanged += HandlePlayerChanged;
    }
    private void OnDisable() {
        GameState.Instance.OnPlayerChanged -= HandlePlayerChanged;
    }
    public void Subscribe(GamePhaseManager phaseManager) {
        phaseManager.OnPhaseChanged += HandleGamePhaseChanged;
    }
    private void HandlePlayerChanged(Player newPlayer) {
        Debug.Log("Player changed! Color: " + newPlayer.Color);
        SetBackgroundColor(newPlayer.Color);

        if (_currentPlayer != null) {
            _currentPlayer.OnPlayerInfoChange -= UpdatePlayerInfo;
        }
        _currentPlayer = newPlayer;
        if (_currentPlayer != null) {
            _currentPlayer.OnPlayerInfoChange += UpdatePlayerInfo;
        }
    }
    private void UpdatePlayerInfo(Player player) {
        Debug.Log("Updating player info on panel");
        _combatIconPanel.ClearPanel();
        foreach(var combatCard in player.CombatCards) {
            _combatIconPanel.AddCardIcon(combatCard);
        }
        _artifactIconPanel.ClearPanel();
        foreach (var artifact in player.ArtifactCards) {
            _artifactIconPanel.AddCardIcon(artifact);
        }
    }
    private void SetBackgroundColor(PlayerColor color) {
        var palette = GameData.PlayerColorPalette;
        switch (color) {
            case PlayerColor.Red: SetBackgroundColor(palette.Red); break;
            case PlayerColor.Green: SetBackgroundColor(palette.Green); break;
            case PlayerColor.Blue: SetBackgroundColor(palette.Blue); break;
            case PlayerColor.Yellow: SetBackgroundColor(palette.Yellow); break;
            default: SetBackgroundColor(_defaultBackgroundColor); break;
        }
    }
    private void SetBackgroundColor(Color color) {
        if (_backgroundImage != null) {
            _backgroundImage.color = new Color(color.r, color.g, color.b, _backgroundImage.color.a);
        } else {
            Debug.LogError("Image color not assigned!");
        }
    }
    private void HandleGamePhaseChanged(GamePhaseBase gamePhase) {
        _currentGamePhaseText.text = gamePhase.Name;
    }
}
