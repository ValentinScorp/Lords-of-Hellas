using System.Collections.Generic;
using UnityEngine;

internal class GamePhaseHeroPlacement : GamePhaseBase
{
    private TokenPlacementManager _placementManager;
    private TurnOrderManager TurnOrderManager;

    internal override string Name => "Heroes Placement Phase";

    internal GamePhaseHeroPlacement(GamePhaseManager phaseManager, 
                                IReadOnlyList<Player> players)
        : base (phaseManager) {
        TurnOrderManager = new TurnOrderManager(players);
        _placementManager = ServiceLocator.Get<TokenPlacementManager>();
        TurnOrderManager.PlayerChanged += HandlePlayerChanged;
        TurnOrderManager.NoPlayersLeft += ProceedNextPhase;
    }
    internal override void OnEnter() {
        TurnOrderManager.StartPlacement();
        _placementManager.StartPlacement(TurnOrderManager.CurrentPlayer, HandlePlacementCompleted);
    }
       
    internal override void OnExit() {
    }
    private void HandlePlacementCompleted(Player player) {
        player.ApplyHeroStartingBonus(StartingBonusApplied);
        TurnOrderManager.PrevPlayer();
    }
    private void ProceedNextPhase() {
        PhaseManager.NextPhase(this);
    }
    private void HandlePlayerChanged(Player player) {
        GameContext.Instance.CurrentPlayer = player;
        player.TakeCombatCards(1);
        _placementManager.StartPlacement(TurnOrderManager.CurrentPlayer, HandlePlacementCompleted);
    }
    private void StartingBonusApplied()
    {
        // TODO Remove this
    }
}
