
using System;

public class HopliteMoveAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private RegularAction _regularAction;
    private TokenView _selectedToken;
    private HopliteModel _selectedHoplite;
    private Action _onComplete;
    public HopliteMoveAction()
    {
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
    }
    public void Start(RegularAction regularAction, Action onComplete = null)
    {
        _regularAction = regularAction;
        _onComplete = onComplete;
        _tokenSelector.WaitTokenSelection(regularAction.Player.Color, TokenType.HopliteStack, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        if (token.ViewModel.Model is HopliteStackModel hopliteStack) {
            if (_regularAction.TryTakeUnmovedHoplite(hopliteStack, out var unmovedHoplite)) {
                _selectedToken = token;
                _selectedHoplite = unmovedHoplite;

                _tokenMover.CreateGhostToken(token);
                _tokenMover.CatchNeibRegionPoint(token, token.RegionId, HandleHopliteMove);
                return;
            }
        }  
        _tokenSelector.WaitTokenSelection(_regularAction.Player.Color, TokenType.HopliteStack, HandleSelection);        
    }

    private void HandleHopliteMove(SpawnPoint spawnPoint)
    {
        PlaceHoplite(_selectedHoplite, spawnPoint);
        _regularAction.HoplitesSteps--;
    }
    private void PlaceHoplite(HopliteModel hoplite, SpawnPoint spawnPoint)
    {
        var regDataRegistry = GameContext.Instance.RegionDataRegistry;
        if (regDataRegistry.MoveHopliteUnit(hoplite, spawnPoint.RegionId)) {
            if (regDataRegistry.TryGetToken(spawnPoint.RegionId, TokenType.HopliteStack, hoplite.PlayerColor, out var hopliteStack)) {
                
            }
        }
        // ServiceLocator.Get<RegionsView>().PlaceTokenAtSpawn(token, spawnPoint);      
        // ServiceLocator.Get<TokenVisualChanger>().PrepareTokenPlacement(_heroToken, _heroToken.PlayerColor);


        _tokenMover.DestroyVisuals();
    }
    private void HandleMoveComplete()
    {
        
    }
}
