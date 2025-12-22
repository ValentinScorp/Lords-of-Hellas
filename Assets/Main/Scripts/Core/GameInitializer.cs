using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private UserInputController _userInputController;
    [SerializeField] private RegionsView _regionViewManager;
    [SerializeField] private TokenPlacementView _tokenPlacementView;
    [SerializeField] private GameObject _templePoolUIPanel;
    [SerializeField] private GameObject _boardSurface;
    [SerializeField] private Button _startPlacementButton;
    [SerializeField] private CardSelectPanelView _cardSelectPanelView;
    [SerializeField] private PlayerInfoPanel _playerInfoPanelView;
    [SerializeField] private RegularActionPanel _regularActionPanel;
    [SerializeField] private RegionInfoPanel _regionInfoUiPanel;
    
    private RouteLink _routeLink;
    private GameManager _gameManager;
    private TokenPlacementManager _tokenPlacementManager;
    private TokenPlacementViewModel _tokenPlacementViewModel;
    private RegionDataRegistry _regionDataRegistry = new();
    private CardSelectPanel _cardSelectPanel;
    private TokenSelector _tokenSelector;
    private TokenMover _tokenMover;
    private RaycastIntersector _raycastBoard;
    private UiRegistry _uiRegistry = new();

    public SelectMgr _clickMgr;


    private void Awake() 
    {
        CheckIfExist(_userInputController, "_userInputController");
        CheckIfExist(_regionViewManager, "_regionViewManager");
        CheckIfExist(_tokenPlacementView, "_tokenPlacementView");
        CheckIfExist(_templePoolUIPanel, "_templePoolUIPanel");
        CheckIfExist(_boardSurface, "_boardSurface");
        CheckIfExist(_startPlacementButton, "_startPlacementButton");
        CheckIfExist(_cardSelectPanelView, "_cardSelectPanelView");
        CheckIfExist(_regularActionPanel, "_regularActionPanel");

        ServiceLocator.Register(_canvas);
        ServiceLocator.Register(_userInputController);

        GameData.Instance.Initialize();
        GameState.Instance.Initialize();

        _uiRegistry.Register(_regionInfoUiPanel);
        ServiceLocator.Register(_uiRegistry);

        ServiceLocator.Register(_regionDataRegistry);

       _raycastBoard = new RaycastIntersector(Camera.main, 
                                                _boardSurface, 
                                                1 << LayerMask.NameToLayer("BoardSurface"));

        _tokenPlacementManager = new TokenPlacementManager(_regionViewManager);
        _tokenPlacementViewModel = new TokenPlacementViewModel();
        _tokenPlacementView.Subscribe(_tokenPlacementViewModel);

        _tokenPlacementViewModel.Initialize(_tokenPlacementManager,
                                            _userInputController,
                                            _raycastBoard);

        ServiceLocator.Register(_raycastBoard);

        _clickMgr = new SelectMgr(Camera.main, _userInputController);
        ServiceLocator.Register(_clickMgr);

        _cardSelectPanel = new CardSelectPanel();
        _cardSelectPanelView.Initialize(_cardSelectPanel);

        _gameManager = new GameManager(GameData.Instance, _tokenPlacementManager, _cardSelectPanel);
        _playerInfoPanelView.Subscribe(_gameManager.GamePhaseManager);

        _tokenSelector = new TokenSelector();
        ServiceLocator.Register(_tokenSelector);

        _tokenMover = new TokenMover();
        ServiceLocator.Register(_tokenMover);

        _startPlacementButton.onClick.AddListener(_gameManager.StartGame);
        _gameManager.OnGameStarted += () => _startPlacementButton.interactable = false;        

        var tokenPrefabFactory = new TokenPrefabFactory();
        ServiceLocator.Register(tokenPrefabFactory);

        var regularActionController = new RegularActionController(_regularActionPanel);
        var regularActionService = new RegularActionService(regularActionController);
        ServiceLocator.Register(regularActionService);

        _routeLink = new RouteLink();
        _routeLink.Create(new Vector3(0f, 0f, 0f), new Vector3(10f, 0f, 0f), PlayerColor.Red);
    }
    private void Update() {
        _tokenPlacementViewModel.UpdatePlacement();
        _tokenMover.Update();
    }
    private void CheckIfExist(object parameter, string message) {
        if (parameter == null) Debug.LogWarning($"No {message} assigned in GameInitializer!");
    }
    private void OnDestroy() {
        _tokenPlacementView.Unsubscribe();
        _tokenPlacementViewModel.Unsubscribe();
    }
}
