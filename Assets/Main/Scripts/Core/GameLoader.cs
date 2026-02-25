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
    [SerializeField] private RegionInfoUi _regionInfoUiPanel;

    private GameManager _gameManager;
    private CardSelectPanel _cardSelectPanel;
    private TokenSelector _tokenSelector;
    private TokenMover _tokenMover;
    private TokenPlacer _tokenPlacer;
    private RaycastIntersector _raycastBoard;

    public ObjectsHitDetector _objectsHitDetector;

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

        // ServiceLocator.Register(new RouteArcBuilder());

        ServiceLocator.Register(new TokenViewRegistry());
        ServiceLocator.Register(new TokenPlacementManager());

        _raycastBoard = new RaycastIntersector(Camera.main, 
                                                _boardSurface, 
                                                1 << LayerMask.NameToLayer("BoardSurface"));
      
        ServiceLocator.Register(_raycastBoard);

        _objectsHitDetector = new ObjectsHitDetector(Camera.main, _userInputController);
        ServiceLocator.Register(_objectsHitDetector);

        _cardSelectPanel = new CardSelectPanel();
        _cardSelectPanelView.Initialize(_cardSelectPanel);

        _gameManager = new GameManager(_cardSelectPanel);

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

    }
    private void Start() 
    {
        var regionInfoUiCtlr = ServiceLocator.Get<RegionInfoUiCtlr>();
        // ServiceLocator.Get<ObjectsHitDetector>().RegisterRegionInfoController(regionInfoUiCtlr);
    }
    private void Update() {
    }
    private void CheckIfExist(object parameter, string message) {
        if (parameter == null) Debug.LogWarning($"No {message} assigned in GameInitializer!");
    }
    private void OnDestroy() {
        _startPlacementButton.onClick.RemoveListener(_gameManager.StartGame);
    }
}
