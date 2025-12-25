using UnityEngine;

public class HeroMoveAction
{
    private readonly TokenSelector _tokenSelector;
    private readonly TokenMover _tokenMover;
    private int _maxSteps;
    private TokenView _heroToken;
    private MoveRoute _moveRoute;
    public HeroMoveAction()
    {
        _tokenSelector = ServiceLocator.Get<TokenSelector>();
        _tokenMover = ServiceLocator.Get<TokenMover>();
        _moveRoute = new MoveRoute();
    }
    public void Start(Player player)
    {
        _maxSteps = player.Hero.Speed;   
            
        _tokenSelector.WaitTokenSelection(player.Color, TokenType.Hero, HandleSelection);
    }
    private void HandleSelection(TokenView token)
    {
        _heroToken = token;
        
        _moveRoute.SetSteps(_maxSteps); 
        _moveRoute.AddRouteNode(token.RegionId, token.SpawnPoint);

        _tokenMover.CreateGhostToken(token);
        _tokenMover.ProceedStep(token, token.Model.RegionId, HandleStep);
    }
    private void HandleStep(SpawnPoint spawnPoint)
    {
        _moveRoute.AddRouteNode(spawnPoint.RegionId, spawnPoint);

        if (_moveRoute.Complete) {
            HandleMoveComplete(spawnPoint);
        } else {
            _tokenMover.ProceedStep(_heroToken, spawnPoint.RegionId, HandleStep);
        }
    }
    private void HandleMoveComplete(SpawnPoint spawnPoint)
    {
        ServiceLocator.Get<RegionsView>().PlaceTokenAtSpawn(_heroToken, spawnPoint);      
        ServiceLocator.Get<TokenVisualChanger>().PrepareTokenPlacement(_heroToken, _heroToken.PlayerColor);

        ServiceLocator.Get<RegionDataRegistry>().RegisterToken(spawnPoint.RegionId, _heroToken.Model);

        _tokenMover.DestroyVisuals();
        _moveRoute.Clear();

        Debug.Log("Hero move complete!");
    }
}
