
using System;

public class HopliteMoveAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private TokenView _selectedToken;
    private HopliteModel _selectedHoplite;
    private Player _player;
    private Action _onComplete;
    public HopliteMoveAction()
    {
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
    }
    public void Start(Player player, Action onComplete = null)
    {
        _player = player;
        _onComplete = onComplete;
        _tokenSelector.WaitTokenSelection(player.Color, TokenType.HopliteStack, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        if (token.ViewModel.Model is HopliteStackModel hopliteStack) {
            if (hopliteStack.TryTakeUnmovedHoplite(out var unmovedHoplite)) {
                _selectedToken = token;
                _selectedHoplite = unmovedHoplite;

                _tokenMover.CreateGhostToken(token);
                _tokenMover.CatchNeibRegionPoint(token, token.RegionId, HandleHopliteMove);
                return;
            }
        }  
        _tokenSelector.WaitTokenSelection(_player.Color, TokenType.HopliteStack, HandleSelection);        
    }

    private void HandleHopliteMove(TokenNest spawnPoint)
    {
        PlaceHoplite(_selectedHoplite, spawnPoint);
        // TODO derc hoplite counter
    }
    private void PlaceHoplite(HopliteModel hoplite, TokenNest spawnPoint)
    {
        var regDataRegistry = GameContext.Instance.RegionDataRegistry;
        if (regDataRegistry.TryMoveHoplite(hoplite, spawnPoint.RegionId)) {
            if (regDataRegistry.TryFindToken(spawnPoint.RegionId, TokenType.HopliteStack, hoplite.PlayerColor, out var hopliteStack)) {
                // TODO update hoplite placement
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
