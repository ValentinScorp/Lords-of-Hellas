using UnityEngine;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private UserInputController _userInputController;
    [SerializeField] private RegionsView _regionsView;
    [SerializeField] private GameObject _templePoolUIPanel;
    [SerializeField] private GameObject _boardSurface;
    [SerializeField] private Button _startPlacementButton;
    [SerializeField] private CardSelectPanelView _cardSelectPanelView;
    [SerializeField] private PlayerInfoUi _playerInfoPanelView;
    [SerializeField] private RegionInfoUi _regionInfoUiPanel;

    private GameManager _gameManager;
    private TokenPlacementManager _tokenPlacementManager;
    private CardSelectPanel _cardSelectPanel;
    private TokenSelector _tokenSelector;
    private TokenMover _tokenMover;
    private TokenPlacer _tokenPlacer;
    private RaycastIntersector _raycastBoard;
    private UiRegistry _uiRegistry = new();

    public ObjectsHitDetector _objectsHitDetector;
    public RegionsViewModel _regionsViewModel;


    private void Awake() 
    {
        CheckIfExist(_userInputController, "_userInputController");
        CheckIfExist(_regionsView, "_regionViewManager");
        CheckIfExist(_templePoolUIPanel, "_templePoolUIPanel");
        CheckIfExist(_boardSurface, "_boardSurface");
        CheckIfExist(_startPlacementButton, "_startPlacementButton");
        CheckIfExist(_cardSelectPanelView, "_cardSelectPanelView");

        ServiceLocator.Register(_canvas);
        ServiceLocator.Register(_userInputController);

        GameContent.Instance.Initialize();
        GameConfig.Instance.Initialize();
        GameContext.Instance.Initialize();

        _uiRegistry.Register(_regionInfoUiPanel);
        ServiceLocator.Register(_uiRegistry);

        ServiceLocator.Register(new TokenPlacementViewModel());

        _raycastBoard = new RaycastIntersector(Camera.main, 
                                                _boardSurface, 
                                                1 << LayerMask.NameToLayer("BoardSurface"));

        _tokenPlacementManager = new TokenPlacementManager(_regionsView);
      
        ServiceLocator.Register(_raycastBoard);

        _objectsHitDetector = new ObjectsHitDetector(Camera.main, _userInputController);
        ServiceLocator.Register(_objectsHitDetector);

        _cardSelectPanel = new CardSelectPanel();
        _cardSelectPanelView.Initialize(_cardSelectPanel);

        _gameManager = new GameManager(GameContent.Instance, _tokenPlacementManager, _cardSelectPanel);
        _playerInfoPanelView.Subscribe(_gameManager.GamePhaseManager);

        ServiceLocator.Register(new TokenVisualChanger(GameContent.TokenMaterialPalette));

        _tokenSelector = new TokenSelector();
        ServiceLocator.Register(_tokenSelector);

        _tokenMover = new TokenMover();
        ServiceLocator.Register(_tokenMover);

        _tokenPlacer = new TokenPlacer();
        ServiceLocator.Register(_tokenPlacer);

        _startPlacementButton.onClick.AddListener(_gameManager.StartGame);
        _gameManager.OnGameStarted += () => _startPlacementButton.interactable = false;

        var tokenPrefabFactory = new TokenFactory();
        ServiceLocator.Register(tokenPrefabFactory);

        _regionsViewModel = new RegionsViewModel();
        ServiceLocator.Register(_regionsViewModel);
    }
    private void Start() 
    {
        var regionInfoUiCtlr = ServiceLocator.Get<RegionInfoUiCtlr>();
        ServiceLocator.Get<ObjectsHitDetector>().RegisterRegionInfoController(regionInfoUiCtlr);
    }
    private void Update() {
        _tokenMover.Update();
    }
    private void CheckIfExist(object parameter, string message) {
        if (parameter == null) Debug.LogWarning($"No {message} assigned in GameInitializer!");
    }
    private void OnDestroy() {
        _startPlacementButton.onClick.RemoveListener(_gameManager.StartGame);
        _regionsViewModel.Dispose();

    }
}
