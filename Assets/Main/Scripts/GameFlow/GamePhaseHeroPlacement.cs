using System.Collections.Generic;

public class GamePhaseHeroPlacement : GamePhaseBase
{
    private TokenPlacementPresenter _tokenPlacementPresenter;
    private TurnOrderManager TurnOrderManager;

    public override string Name => "Heroes Placement Phase";

    public GamePhaseHeroPlacement(GamePhaseManager phaseManager, 
                                IReadOnlyList<Player> players)
        : base (phaseManager) {
        TurnOrderManager = new TurnOrderManager(players);
        _tokenPlacementPresenter = ServiceLocator.Get<TokenPlacementPresenter>();
        TurnOrderManager.OnPlayerChanged += HandleNextPlayer;
        TurnOrderManager.OnNoPlayersLeft += ProceedNextPhase;
    }
    public override void Enter() {
        TurnOrderManager.StartPlacement();
        _tokenPlacementPresenter.StartPlacement(TurnOrderManager.CurrentPlayer, HandlePlacementCompleted);
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
        _tokenPlacementPresenter.StartPlacement(TurnOrderManager.CurrentPlayer, HandlePlacementCompleted);
    } 
}
