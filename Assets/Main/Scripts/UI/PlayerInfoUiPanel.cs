using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUiPanel : UiPanel
{
    [SerializeField] private HopliteInfoUiPanel _hoplitesPanel;
    [SerializeField] private Button _testButton;
    
    // private Player _player;
    protected override void Awake()
    {
        base.Awake();
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

        base.OnDestroy();
    }
    internal void DiplayPlayerInfo(Player player)
    {
        Show(true);
        _hoplitesPanel.Show(true);
        Unbind();
        Bind(player);
    }
    private void Bind(Player player)
    {
        _hoplitesPanel.Bind(player.HopliteManager);
    }
    private void Unbind()
    {
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
            default: SetBackgroundColor(palette.Grey); break;
        }
    }
    private void SetBackgroundColor(Color color)
    {
        // if (_backgroundImage != null) {
        //     _backgroundImage.color = new Color(color.r, color.g, color.b, _backgroundImage.color.a);
        // } else {
        //     Debug.LogError("Image color not assigned!");
        // }
    }
    private void AddHoplite()
    {
        
    }
}
