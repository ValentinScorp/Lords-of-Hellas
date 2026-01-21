using System;

public class CmdPlaceHopliteStartup
{
    private HopliteModel _hoplite;
    public void Init(HopliteModel hoplite)
    {
        _hoplite = hoplite;
    }
    public bool CanExecute()
    {
        return GameContext.Instance.RegionDataRegistry.HoplitesCount(_hoplite.PlayerColor) < 2;
    }
    public void Execute(Action<CommandResult> CmdComplete)
    {
        if (!CanExecute()) {
            CmdComplete?.Invoke(CommandResult.Fail("Hero already placed."));
            return;
        }

        var _ghostToken = ServiceLocator.Get<TokenFactory>().CreateGhostToken(_hoplite);

        CmdComplete?.Invoke(CommandResult.Ok());
    }
}
