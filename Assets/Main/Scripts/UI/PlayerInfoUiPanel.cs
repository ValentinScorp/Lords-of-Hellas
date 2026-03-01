using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUiPanel : UiPanel
{
    [SerializeField] private HopliteInfoUiPanel _hoplitesPanel;
    [SerializeField] private Button _testButton;
    [SerializeField] private Image _backgroundImage;
    
    // private Player _player;
    private static Color _defaultBackgroundColor = Color.white;
    protected override void Awake()
    {
        base.Awake();

        SetBackgroundColor(_defaultBackgroundColor);
    }
    private void Start()
    {
        _testButton?.onClick.AddListener(AddHoplite);
        Show(false);        
    }
    protected override void OnDestroy()
    {
        _testButton?.onClick.RemoveListener(AddHoplite);
        Unbind();

        // _player = null;

        base.OnDestroy();
    }
    internal void DiplayPlayerInfo(Player player)
    {
        Show(true);
        Unbind();
        Bind(player);
    }
    internal void Bind(Player player)
    {
        SetBackgroundColor(player.Color);
        _hoplitesPanel.Bind(player.HopliteManager);
    }
    internal void Unbind()
    {
        SetBackgroundColor(PlayerColor.Gray);
        _hoplitesPanel.Unbind();       
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
    private void AddHoplite()
    {
        
    }
}
