using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TokenPlacementView : MonoBehaviour
{
    [SerializeField] private Button _placeHeroButton;
    [SerializeField] private Button _placeHopliteButton;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;

    private TokenPlacementPresenter _tokenPlacementPresenter;

    private void Start()
    {
        ShowPanel(false);
        _tokenPlacementPresenter = ServiceLocator.Get<TokenPlacementPresenter>();        
        _tokenPlacementPresenter.OnStartPlacement += HandleStartPlacement;
        _tokenPlacementPresenter.RefreshAction += UpdateButtonInteractability;
        _placeHeroButton.onClick.AddListener(_tokenPlacementPresenter.PlaceHero);
        _placeHopliteButton.onClick.AddListener(_tokenPlacementPresenter.PlaceHoplite);
        _okButton.onClick.AddListener(_tokenPlacementPresenter.OkPlacement);
        _cancelButton.onClick.AddListener(_tokenPlacementPresenter.UndoPlcaement);
    }
    private void OnDestroy() {
        if (_tokenPlacementPresenter != null) {
            _tokenPlacementPresenter.OnStartPlacement -= HandleStartPlacement;
            _tokenPlacementPresenter.RefreshAction -= UpdateButtonInteractability;
            _placeHeroButton.onClick.RemoveListener(_tokenPlacementPresenter.PlaceHero);
            _placeHopliteButton.onClick.RemoveListener(_tokenPlacementPresenter.PlaceHoplite);
            _okButton.onClick.RemoveListener(_tokenPlacementPresenter.OkPlacement);
            _cancelButton.onClick.RemoveListener(_tokenPlacementPresenter.UndoPlcaement);
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
        _placeHeroButton.interactable = _tokenPlacementPresenter.CanPlaceHero();
        _placeHopliteButton.interactable = _tokenPlacementPresenter.CanPlaceHoplite();
        _okButton.interactable = _tokenPlacementPresenter.CanComplete();
        _cancelButton.interactable = _tokenPlacementPresenter.CanUndoPlacement();
    }    
}
