public abstract class GamePhaseBase
{
    protected readonly GamePhaseManager PhaseManager;

    protected GamePhaseBase(GamePhaseManager phaseManager) {
        PhaseManager = phaseManager;
    }

    public abstract string Name { get; }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}