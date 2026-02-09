using System.Collections.Generic;
using UnityEngine;

public class GamePhaseHeroPlacement : GamePhaseBase
{
    private TokenPlacementManager _tokenPlacementPresenter;
    private TurnOrderManager TurnOrderManager;

    public override string Name => "Heroes Placement Phase";

    public GamePhaseHeroPlacement(GamePhaseManager phaseManager, 
                                IReadOnlyList<Player> players)
        : base (phaseManager) {
        TurnOrderManager = new TurnOrderManager(players);
        _tokenPlacementPresenter = ServiceLocator.Get<TokenPlacementManager>();
        TurnOrderManager.PlayerChanged += HandlePlayerChanged;
        TurnOrderManager.NoPlayersLeft += ProceedNextPhase;
    }
    public override void OnEnter() {
        TurnOrderManager.StartPlacement();
        _tokenPlacementPresenter.StartPlacement(TurnOrderManager.CurrentPlayer, HandlePlacementCompleted);
    }
       
    public override void OnExit() {
    }
    private void HandlePlacementCompleted() {
        TurnOrderManager.PrevPlayer();
    }
    private void ProceedNextPhase() {
        PhaseManager.NextPhase(this);
    }
    private void HandlePlayerChanged(Player player) {
        GameContext.Instance.CurrentPlayer = player;
        player.TakeCombatCards(1);
        player.ApplyHeroStartingBonus(StartingBonusApplied);
        
    }
    private void StartingBonusApplied()
    {
        _tokenPlacementPresenter.StartPlacement(TurnOrderManager.CurrentPlayer, HandlePlacementCompleted);
    }
}
