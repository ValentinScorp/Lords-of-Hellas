using System;

internal class TokenPlacementManager
{
    private PlacementController _placementController;
    private TokenPlacementModel _model = new();

    private bool _isBusy;
    internal event Action PlacementStarted;
    internal event Action PlacementCompleted;
    internal event Action RefreshAction;
    internal Action<Player> _placementCompleted;

    internal void StartPlacement(Player player, Action<Player> placementCompleted)
    {
        if (_isBusy) return;
        if (_placementController is null) {
            _placementController = new();            
        }
        _placementCompleted = placementCompleted;
        _model.Player = player;

        PlacementStarted?.Invoke();
    }
    internal bool CanPlaceHero() 
    {
        if (_isBusy) return false;

        return _model.CanPlaceHero();
    }
    internal bool CanPlaceHoplite() 
    {
        if (_isBusy) return false;

        return _model.CanPlaceHoplite();
    }
    internal bool CanComplete()
    {
        return _model.CanComplete();
    }
    internal bool CanUndoPlacement()
    {
        // TODO
        return false;
    }
    internal void PlaceHero() 
    {
        if (CanPlaceHero()) {
            _isBusy = true;
            _placementController.Start(_model.Hero, new RulesCheckerStartup(), HandlePlacementComplete);
        }
        RefreshAction?.Invoke();
    }
    internal void PlaceHoplite() 
    { 
        if (CanPlaceHoplite()) {
            _isBusy = true;
            _placementController.Start(_model.Hoplite, new RulesCheckerStartup(), HandlePlacementComplete);
        }
        RefreshAction?.Invoke();
    }
    internal void UndoPlcaement()
    {
        // TODO
    }
    internal void OkPlacement()
    {
        PlacementCompleted?.Invoke();
        _placementCompleted?.Invoke(_model.Player);
    }
    private void HandlePlacementComplete(TokenModel token)
    {        
        _isBusy = false;
        RefreshAction?.Invoke();       
    }
}
