using System;

public class CmdPlaceHopliteStartup
{
    private HopliteModel _hoplite;
    private TokenDragger _tokenDragger;
    private TokenNestHitDetector _tokenNestHitDetector = new();
    private Action<CommandResult> _CommandCompleted; 
    public void Init(HopliteModel hoplite)
    {
        if (_tokenDragger == null) {
            _tokenDragger = new TokenDragger();
        }
        _hoplite = hoplite;
    }
    public bool CanExecute()
    {
        return GameContext.Instance.RegionDataRegistry.HoplitesCount(_hoplite.PlayerColor) < 2;
    }
    public void Execute(Action<CommandResult> CmdComplete)
    {
        if (!CanExecute()) {
            CmdComplete?.Invoke(CommandResult.Fail("No more hoplites to place!"));
            return;
        }

       _CommandCompleted = CmdComplete;
        var _ghostToken = ServiceLocator.Get<TokenFactory>().CreateGhostToken(_hoplite);
        _tokenDragger.SetTarget(_ghostToken);
        _tokenNestHitDetector.ListenHits(HandleHitedNest);
    }
    public void HandleHitedNest(TokenNest nest)
    {
         if (_tokenDragger.TryRemoveTarget(out var token)) {
            
            GameContext.Instance.RegionDataRegistry.TryPlace(nest.RegionId, token.Model);
            token.Place(nest);
            _CommandCompleted?.Invoke(CommandResult.Ok());
        }
    }
}
