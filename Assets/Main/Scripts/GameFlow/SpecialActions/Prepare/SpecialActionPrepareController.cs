using System;
using UnityEngine;

internal class SpecialActionPrepareController
{
    private Player _player;
    private Action<Player> _Completed;
    private SpecialActionPrepareModel _model = new();
    private PreparePanel _uiPanel;
    internal SpecialActionPrepareController()
    {
        _uiPanel = SceneUiRegistry.Get<PreparePanel>();
        if (_uiPanel is null) {
            Debug.LogWarning("Can't get UI Panel for Special Action Prepare!");
        }
    }
    internal void Launch(Player player, Action<Player> Completed)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));
        if (Completed == null) throw new ArgumentNullException(nameof(Completed));

        _player = player;
        _Completed = Completed;
        _model.Reset();

        _uiPanel?.Bind(this);
        _uiPanel?.Show(true);
    }
   
    internal void OnHealInjuryPressed()
    {
        Shutdown(_player);
    }
    internal void OnDrawCombatCardPressed()
    {
        _player.TakeCombatCards(1);
        CheckIfShutdown();
    }
    internal void OnRecruitHoplitePressed()
    {
        if (!_player.Hero.IsOnBoard()) return;

        if (_player.TryTakeHoplite(out var hoplite)) {
            if (GameContext.Instance.RegionRegistry.TryPlace(hoplite, _player.Hero.RegionId)) {
                CheckIfShutdown();
            }
        }       
    }
    private void CheckIfShutdown()
    {
        _model.NextOption();
        if (_model.NoOptionsLeft) {
            Shutdown(_player);
        }
    }
    private void Shutdown(Player player)
    {
        _uiPanel.Show(false);
        _uiPanel.Unbind(this);
        _uiPanel = null;

        _Completed?.Invoke(player);
        _player = null;
        _Completed = null;  
    }
}
