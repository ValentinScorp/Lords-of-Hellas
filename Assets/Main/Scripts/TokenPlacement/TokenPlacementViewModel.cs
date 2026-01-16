using System;

public class TokenPlacementViewModel
{
    private Player _curPlayer;
    private CmdPlaceHeroStartup _cmdPlaceHero = new();
    private CmdPlaceHopliteStartup _cmdPlaceHoplite = new();
    public event Action OnStartPlacement;

    public void StartPlacement(Player player)
    {
        _curPlayer = player;
        _cmdPlaceHero.Init(player.Hero);
        _cmdPlaceHoplite.Init(player.TakeHoplite());

        OnStartPlacement?.Invoke();
    }
    public bool CanPlaceHero() { return _cmdPlaceHero.CanExecute(); }
    public bool CanPlaceHoplite() { return _cmdPlaceHoplite.CanExecute(); }
    public void PlaceHero() { _cmdPlaceHero.Execute(); }
    public void PlaceHoplite() { _cmdPlaceHoplite.Execute(); }

}
