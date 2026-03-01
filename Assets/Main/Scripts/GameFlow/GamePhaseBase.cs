internal abstract class GamePhaseBase
{
    protected readonly GamePhaseManager PhaseManager;

    protected GamePhaseBase(GamePhaseManager phaseManager) {
        PhaseManager = phaseManager;
    }

    internal abstract string Name { get; }

    internal virtual void OnEnter() { }
    internal virtual void OnExit() { }
}