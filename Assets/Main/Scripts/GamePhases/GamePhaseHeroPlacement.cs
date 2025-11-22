using System.Collections.Generic;

public class GamePhaseHeroPlacement : GamePhaseBase
{
    private TokenPlacementManager PlacementManager;
    private PlayerTurnOrderManager PlayerPlacementOrder;

    public override string Name => "Heroes Placement Phase";

    public GamePhaseHeroPlacement(GamePhaseManager phaseManager, 
                                IReadOnlyList<Player> players, 
                                TokenPlacementManager placementManager)
        : base (phaseManager) {
        PlacementManager = placementManager;
        PlayerPlacementOrder = new PlayerTurnOrderManager(players);
        PlayerPlacementOrder.OnPlayerChanged += HandleNextPlayer;
        PlayerPlacementOrder.OnNoPlayersLeft += ProceedNextPhase;
    }
    public override void Enter() {
        PlayerPlacementOrder.StartPlacement();
        InitPlacement(PlayerPlacementOrder.CurrentPlayer);
        PlacementManager.OnPlacementCompleted += HandlePlacementCompleted;
    }
    private void  InitPlacement(Player player) {
        PlacementManager.InitiatePlacing(player, maxHeroes: 1, maxHoplites: 2);
    }    
    public override void Exit() {
        PlacementManager.OnPlacementCompleted -= HandlePlacementCompleted;
    }
    private void HandlePlacementCompleted() {
        PlayerPlacementOrder.PrevPlayer();
    }
    private void ProceedNextPhase() {
        PhaseManager.NextPhase(this);
    }
    private void HandleNextPlayer(Player player) {
        GameState.Instance.CurrentPlayer = player;
        InitPlacement(player);
    }    
}
