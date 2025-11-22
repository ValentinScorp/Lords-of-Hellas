using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Profiling.HierarchyFrameDataView;

public class TokenPlacementView : MonoBehaviour
{
    [SerializeField] private Button _placeHeroButton;
    [SerializeField] private Button _placeHopliteButton;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;

    private TokenPlacementViewModel _tokenPlacementViewModel;
    private bool _isSubscribed = false;
    public void ShowPanel(bool show) {
        //Debug.Log("ShowPanel: " + show);
        gameObject.SetActive(show);
    }
    public void UpdateButtonInteractability(TokenPlacementTracker tracker) {
        _placeHeroButton.interactable = tracker.CanPlace(TokenType.Hero);
        _placeHopliteButton.interactable = tracker.CanPlace(TokenType.Hoplite);
        _okButton.interactable = tracker.AllPlaced;
    }
    public void Subscribe(TokenPlacementViewModel viewModel) {
        if (_isSubscribed || viewModel == null) return;

        _tokenPlacementViewModel = viewModel;
        _isSubscribed = true;

        viewModel.OnTokenPlacementActive += ShowPanel;
        viewModel.OnHeroButtonInteractableChanged += interactable => _placeHeroButton.interactable = interactable;
        viewModel.OnHopliteButtonInteractableChanged += interactable => _placeHopliteButton.interactable = interactable;
        viewModel.OnOkButtonInteractableChanged += interactable => _okButton.interactable = interactable;

        _placeHeroButton.onClick.AddListener(() => viewModel.HandleTokenPlace(TokenType.Hero));
        _placeHopliteButton.onClick.AddListener(() => viewModel.HandleTokenPlace(TokenType.Hoplite));
        _okButton.onClick.AddListener(() => viewModel.FinalizePlacement());
        _cancelButton.onClick.AddListener(() => viewModel.CancelPlacement());

        gameObject.SetActive(false);
    }
    public void Unsubscribe() {
        if (!_isSubscribed || _tokenPlacementViewModel == null) return;

        _isSubscribed = false;        

        _tokenPlacementViewModel.OnTokenPlacementActive -= ShowPanel;
        _tokenPlacementViewModel.OnHeroButtonInteractableChanged -= interactable => _placeHeroButton.interactable = interactable;
        _tokenPlacementViewModel.OnHopliteButtonInteractableChanged -= interactable => _placeHopliteButton.interactable = interactable;
        _tokenPlacementViewModel.OnOkButtonInteractableChanged -= interactable => _okButton.interactable = interactable;

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
