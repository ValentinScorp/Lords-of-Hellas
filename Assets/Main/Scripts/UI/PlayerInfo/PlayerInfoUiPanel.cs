using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUiPanel : UiPanel
{
    [SerializeField] private HopliteInfoUiPanel _hoplitesPanel;
    [SerializeField] private PriestInfoUiPanel _priestsPanel;
    [SerializeField] private LandTokenUiPanel _landTokensPanel;
    
    // private Player _player;
    protected override void Awake()
    {
        if (_hoplitesPanel is null) Debug.LogWarning($"PlayerInfoUiPanel parameter {_hoplitesPanel} is null!");
        if (_priestsPanel is null) Debug.LogWarning($"PlayerInfoUiPanel parameter {_priestsPanel} is null!");
        if (_landTokensPanel is null) Debug.LogWarning($"PlayerInfoUiPanel parameter {_landTokensPanel} is null!");
        
        base.Awake();
    }
    private void Start()
    {
         Show(false);        
    }
    protected override void OnDestroy()
    {
        Unbind();

        base.OnDestroy();
    }
    internal void DiplayPlayerInfo(Player player)
    {
        Show(true);
        _hoplitesPanel.Show(true);
        _priestsPanel.Show(true);
        _landTokensPanel?.Show(true);
        Unbind();
        Bind(player);
    }
    private void Bind(Player player)
    {
        _hoplitesPanel.Bind(player.HopliteManager);
        _priestsPanel.Bind(player.PriestManager);
        _landTokensPanel?.Bind(player, ServiceLocator.Get<LandOwnershipWatcher>());
    }
    private void Unbind()
    {
        _hoplitesPanel.Unbind();
        _priestsPanel.Unbind();
        _landTokensPanel?.Unbind();
    }
}
