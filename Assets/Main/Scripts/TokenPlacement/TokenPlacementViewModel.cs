using System;

public class TokenPlacementViewModel
{
    private Player _curPlayer;
    private CmdPlaceHeroStartup _cmdPlaceHero = new();
    private CmdPlaceHopliteStartup _cmdPlaceHoplite = new();
    private bool _isBusy;
    public event Action OnStartPlacement;
    public event Action RefreshAction;

    public void StartPlacement(Player player)
    {
        if (_isBusy) return;

        _curPlayer = player;
        _cmdPlaceHero.Init(player.Hero);
        _cmdPlaceHoplite.Init(player.TakeHoplite());

        OnStartPlacement?.Invoke();
    }
    public bool CanPlaceHero() 
    {
        if (_isBusy) return false;

        return _cmdPlaceHero.CanExecute();
    }
    public bool CanPlaceHoplite() 
    {
        if (_isBusy) return false;

        return _cmdPlaceHoplite.CanExecute();
    }
    public void PlaceHero() 
    {
        if (CanPlaceHero()) {
            _isBusy = true;
            _cmdPlaceHero.Execute(HandleCmdComplete); 
        }
        RefreshAction?.Invoke();
    }
    public void PlaceHoplite() 
    { 
        if (CanPlaceHoplite()) {
            _isBusy = true;
            _cmdPlaceHoplite.Execute(HandleCmdComplete);
            if (_cmdPlaceHoplite.CanExecute()) {
                _cmdPlaceHoplite.Init(_curPlayer.TakeHoplite());
            }
        }
        RefreshAction?.Invoke();
    }

    private void HandleCmdComplete(CommandResult cmdResult)
    {        
        _isBusy = false;
        RefreshAction?.Invoke();
    }
}
