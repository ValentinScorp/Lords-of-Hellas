using System;

public class TokenPlacementPresenter
{
    private PlacementController _placementController;
    private TokenPlacementModel _model = new();

    private bool _isBusy;
    public event Action PlacementStarted;
    public event Action PlacementCompleted;
    public event Action RefreshAction;
    public Action _placementCompleted;

    public void StartPlacement(Player player, Action placementCompleted)
    {
        if (_isBusy) return;
        if (_placementController is null) {
            _placementController = new();            
        }
        _placementCompleted = placementCompleted;
        _model.SetPlayer(player);

        PlacementStarted?.Invoke();
    }
    public bool CanPlaceHero() 
    {
        if (_isBusy) return false;

        return _model.CanPlaceHero();
    }
    public bool CanPlaceHoplite() 
    {
        if (_isBusy) return false;

        return _model.CanPlaceHoplite();
    }
    public bool CanComplete()
    {
        return _model.CanComplete();
    }
    public bool CanUndoPlacement()
    {
        // TODO
        return false;
    }
    public void PlaceHero() 
    {
        if (CanPlaceHero()) {
            _isBusy = true;
            _placementController.Start(_model.Hero, new RulesCheckerStartup(), HandlePlacementComplete);
        }
        RefreshAction?.Invoke();
    }
    public void PlaceHoplite() 
    { 
        if (CanPlaceHoplite()) {
            _isBusy = true;
            _placementController.Start(_model.Hoplite, new RulesCheckerStartup(), HandlePlacementComplete);
        }
        RefreshAction?.Invoke();
    }
    public void UndoPlcaement()
    {
        // TODO
    }
    public void OkPlacement()
    {
        PlacementCompleted?.Invoke();
        _placementCompleted?.Invoke();
    }
    private void HandlePlacementComplete(TokenModel token)
    {        
        _isBusy = false;
        RefreshAction?.Invoke();       
    }
}
