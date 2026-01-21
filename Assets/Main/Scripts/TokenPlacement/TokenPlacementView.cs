using UnityEngine;
using UnityEngine.UI;

public class TokenPlacementView : MonoBehaviour
{
    [SerializeField] private Button _placeHeroButton;
    [SerializeField] private Button _placeHopliteButton;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;

    private TokenPlacementViewModel _tokenPlacementViewModel;

    private void Start()
    {
        ShowPanel(false);
        _tokenPlacementViewModel = ServiceLocator.Get<TokenPlacementViewModel>();        
        _tokenPlacementViewModel.OnStartPlacement += HandleStartPlacement;
        _tokenPlacementViewModel.RefreshAction += UpdateButtonInteractability;
        _placeHeroButton.onClick.AddListener(_tokenPlacementViewModel.PlaceHero);
        _placeHopliteButton.onClick.AddListener(_tokenPlacementViewModel.PlaceHoplite);
    }
    private void OnDestroy() {
        if (_tokenPlacementViewModel != null) {
            _tokenPlacementViewModel.OnStartPlacement -= HandleStartPlacement;
            _tokenPlacementViewModel.RefreshAction -= UpdateButtonInteractability;
            _placeHeroButton.onClick.RemoveListener(_tokenPlacementViewModel.PlaceHero);
            _placeHopliteButton.onClick.RemoveListener(_tokenPlacementViewModel.PlaceHoplite);
        }
    } 
    private void HandleStartPlacement()
    {
        ShowPanel(true);
        UpdateButtonInteractability();
    }
    public void ShowPanel(bool show) {
        gameObject.SetActive(show);
    }
    public void UpdateButtonInteractability() {
        _placeHeroButton.interactable = _tokenPlacementViewModel.CanPlaceHero();
        _placeHopliteButton.interactable = _tokenPlacementViewModel.CanPlaceHoplite();
    }    
}
