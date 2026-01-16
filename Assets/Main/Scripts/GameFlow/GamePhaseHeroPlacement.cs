using System.Collections.Generic;

public class GamePhaseHeroPlacement : GamePhaseBase
{
    private TokenPlacementViewModel _tokenPlacementViewModel;
    private TurnOrderManager TurnOrderManager;

    public override string Name => "Heroes Placement Phase";

    public GamePhaseHeroPlacement(GamePhaseManager phaseManager, 
                                IReadOnlyList<Player> players)
        : base (phaseManager) {
        TurnOrderManager = new TurnOrderManager(players);
        _tokenPlacementViewModel = ServiceLocator.Get<TokenPlacementViewModel>();
        TurnOrderManager.OnPlayerChanged += HandleNextPlayer;
        TurnOrderManager.OnNoPlayersLeft += ProceedNextPhase;
    }
    public override void Enter() {
        TurnOrderManager.StartPlacement();
        _tokenPlacementViewModel.StartPlacement(TurnOrderManager.CurrentPlayer);
    }
       
    public override void Exit() {
    }
    private void HandlePlacementCompleted() {
        TurnOrderManager.PrevPlayer();
    }
    private void ProceedNextPhase() {
        PhaseManager.NextPhase(this);
    }
    private void HandleNextPlayer(Player player) {
        GameContext.Instance.CurrentPlayer = player;
        player.TakeCombatCards(1);
    } 
}
