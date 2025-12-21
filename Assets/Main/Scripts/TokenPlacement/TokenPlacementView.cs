using UnityEngine;
using UnityEngine.UI;

public class TokenPlacementView : MonoBehaviour
{
    [SerializeField] private Button _placeHeroButton;
    [SerializeField] private Button _placeHopliteButton;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;

    private TokenPlacementViewModel _tokenPlacementViewModel;
    private bool _isSubscribed = false;

    private System.Action<bool> _heroInteractableHandler;
    private System.Action<bool> _hopliteInteractableHandler;
    private System.Action<bool> _okInteractableHandler;

    public void ShowPanel(bool show) {
        //Debug.Log("ShowPanel: " + show);
        gameObject.SetActive(show);
    }
    public void UpdateButtonInteractability(TokenPlacementPool tracker) {
        _placeHeroButton.interactable = tracker.CanPlace(TokenType.Hero);
        _placeHopliteButton.interactable = tracker.CanPlace(TokenType.HopliteStack);
        _okButton.interactable = tracker.AllPlaced();
    }
    public void Subscribe(TokenPlacementViewModel viewModel) {
        if (_isSubscribed || viewModel == null) return;

        _tokenPlacementViewModel = viewModel;
        _isSubscribed = true;

        viewModel.OnTokenPlacementActive += ShowPanel;

        _heroInteractableHandler = interactable => _placeHeroButton.interactable = interactable;
        _hopliteInteractableHandler = interactable => _placeHopliteButton.interactable = interactable;
        _okInteractableHandler = interactable => _okButton.interactable = interactable;

        viewModel.OnHeroButtonInteractableChanged += _heroInteractableHandler;
        viewModel.OnHopliteButtonInteractableChanged += _hopliteInteractableHandler;
        viewModel.OnOkButtonInteractableChanged += _okInteractableHandler;

        _placeHeroButton.onClick.AddListener(() => viewModel.HandleTokenPlace(TokenType.Hero));
        _placeHopliteButton.onClick.AddListener(() => viewModel.HandleTokenPlace(TokenType.HopliteStack));
        _okButton.onClick.AddListener(() => viewModel.FinalizePlacement());
        _cancelButton.onClick.AddListener(() => viewModel.CancelPlacement());

        gameObject.SetActive(false);
    }
    public void Unsubscribe() {
        if (!_isSubscribed || _tokenPlacementViewModel == null) return;

        _isSubscribed = false;        

        _tokenPlacementViewModel.OnTokenPlacementActive -= ShowPanel;
        _tokenPlacementViewModel.OnHeroButtonInteractableChanged -= _heroInteractableHandler;
        _tokenPlacementViewModel.OnHopliteButtonInteractableChanged -= _hopliteInteractableHandler;
        _tokenPlacementViewModel.OnOkButtonInteractableChanged -= _okInteractableHandler;

        _tokenPlacementViewModel = null;

        _placeHeroButton?.onClick.RemoveAllListeners();
        _placeHopliteButton?.onClick.RemoveAllListeners();
        _okButton?.onClick.RemoveAllListeners();
        _cancelButton?.onClick.RemoveAllListeners();
    }
    private void OnDestroy() {
        Unsubscribe();        
    }
}
