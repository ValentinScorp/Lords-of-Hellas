using UnityEngine;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{    
    [SerializeField] private UserInputController _userInputController;
    [SerializeField] private RegionViewManager _regionViewManager;
    [SerializeField] private TokenPlacementView _tokenPlacementView;
    [SerializeField] private GameObject _templePoolUIPanel;
    [SerializeField] private GameObject _boardSurface;
    [SerializeField] private Button _startPlacementButton;
    [SerializeField] private CardSelectPanelView _cardSelectPanelView;
    [SerializeField] private PlayerInfoPanel _playerInfoPanelView;
      
    private GameManager _gameManager;
    private RegionDataManager _regionModelManager;
    private TokenPlacementManager _tokenPlacementManager;
    private TokenPlacementViewModel _tokenPlacementViewModel;
    private CardSelectPanel _cardSelectPanel;

    private RaycastIntersector _raycastBoard;

    public RegionDataManager RegionModelManager => _regionModelManager;

    private void Awake() {
        CheckIfExist(_userInputController, "_userInputController");
        CheckIfExist(_regionViewManager, "_regionViewManager");
        CheckIfExist(_tokenPlacementView, "_tokenPlacementView");
        CheckIfExist(_templePoolUIPanel, "_templePoolUIPanel");
        CheckIfExist(_boardSurface, "_boardSurface");
        CheckIfExist(_startPlacementButton, "_startPlacementButton");
        CheckIfExist(_cardSelectPanelView, "_cardSelectPanelView");

        GameData.Instance.Initialize();
        GameState.Instance.Initialize();

        _regionModelManager = new RegionDataManager(_regionViewManager);
                
        _raycastBoard = new RaycastIntersector(Camera.main, 
                                                _boardSurface, 
                                                1 << LayerMask.NameToLayer("BoardSurface"));

        _tokenPlacementManager = new TokenPlacementManager(_regionModelManager, _regionViewManager);
        _tokenPlacementViewModel = new TokenPlacementViewModel();
        _tokenPlacementView.Subscribe(_tokenPlacementViewModel);

        _tokenPlacementViewModel.Initialize(_tokenPlacementManager,
                                            _userInputController,
                                            _raycastBoard);

        _cardSelectPanel = new CardSelectPanel();
        _cardSelectPanelView.Initialize(_cardSelectPanel);

        _gameManager = new GameManager(GameData.Instance, _tokenPlacementManager, _cardSelectPanel);
        _playerInfoPanelView.Subscribe(_gameManager.GamePhaseManager);

        _startPlacementButton.onClick.AddListener(_gameManager.StartGame);
        _gameManager.OnGameStarted += () => _startPlacementButton.interactable = false;
    }
    private void Update() {
        _tokenPlacementViewModel.UpdatePlacement();        
    }
    private void CheckIfExist(object parameter, string message) {
        if (parameter == null) Debug.LogWarning($"No {message} assigned in GameInitializer!");
    }
    private void OnDestroy() {
        _tokenPlacementView.Unsubscribe();
        _tokenPlacementViewModel.Unsubscribe();
    }
}
