using UnityEngine;

public class CmdPlaceHopliteStartup
{
    private HopliteModel _hoplite;
    private RegionsContext _regionsContext;
    public void Init(HopliteModel hoplite)
    {
        _hoplite = hoplite;
        _regionsContext = ServiceLocator.Get<RegionsContext>();
    }
    public void Execute()
    {
        Debug.Log("Execution place Hoplite command!");
        ServiceLocator.Get<TokenPrefabFactory>().CreateGhostToken(_hoplite);
    }
    public bool CanExecute()
    {
        return (_regionsContext.HoplitesCount(_hoplite.PlayerColor) < 2);
    }
}
