using System.Collections.Generic;

public class GamePhaseHeroPlacement : GamePhaseBase
{
    private TokenPlacementManager TokenPlacementManager;
    private TurnOrderManager TurnOrderManager;

    public override string Name => "Heroes Placement Phase";

    public GamePhaseHeroPlacement(GamePhaseManager phaseManager, 
                                IReadOnlyList<Player> players, 
                                TokenPlacementManager placementManager)
        : base (phaseManager) {
        TokenPlacementManager = placementManager;
        TurnOrderManager = new TurnOrderManager(players);
        TurnOrderManager.OnPlayerChanged += HandleNextPlayer;
        TurnOrderManager.OnNoPlayersLeft += ProceedNextPhase;
    }
    public override void Enter() {
        TurnOrderManager.StartPlacement();
        TokenPlacementManager.OnPlacementCompleted += HandlePlacementCompleted;
    }
       
    public override void Exit() {
        TokenPlacementManager.OnPlacementCompleted -= HandlePlacementCompleted;
    }
    private void HandlePlacementCompleted() {
        TurnOrderManager.PrevPlayer();
    }
    private void ProceedNextPhase() {
        PhaseManager.NextPhase(this);
    }
    private void HandleNextPlayer(Player player) {
        GameState.Instance.CurrentPlayer = player;
        InitPlacement(player);
    } 
    private void  InitPlacement(Player player) {
        player.TakeCombatCards(1);
        TokenPlacementManager.InitiatePlacing(player);
    }   
}
