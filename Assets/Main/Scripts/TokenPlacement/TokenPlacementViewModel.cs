using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TokenPlacementViewModel
{
    private UserInputController _userInputController;
    private TokenPlacementManager _tokenPlacementManager;
    private RaycastIntersector _raycastBoard;

    private bool _isSubscribed = false;

    public event Action<bool> OnTokenPlacementActive;
    public event Action<bool> OnHeroButtonInteractableChanged;
    public event Action<bool> OnHopliteButtonInteractableChanged;
    public event Action<bool> OnOkButtonInteractableChanged;

    public void Initialize( TokenPlacementManager tokenPlacementManager,
                            UserInputController userInputController,
                            RaycastIntersector raycastBoard) {
        _tokenPlacementManager = tokenPlacementManager;
        _userInputController = userInputController;
        _raycastBoard = raycastBoard;

        Subscribe();
    }
    public void Subscribe() {
        if (_userInputController == null) return;
        if (_tokenPlacementManager == null) return;
        if (_isSubscribed == true) return;

        _isSubscribed = true;

        // _userInputController.OnMouseClick += HandleClick;
        _tokenPlacementManager.OnPlacementStarted += OnPlacementStarted;
        _tokenPlacementManager.OnPlacementCompleted += OnPlacementCompleted;
        _tokenPlacementManager.OnTokenPlaced += UpdateButtonInteractability;
    }
    public void Unsubscribe() {
        if (_userInputController == null) return;
        if (_tokenPlacementManager == null) return;
        if (_isSubscribed == false) return;

        _isSubscribed = false;
        // _userInputController.OnMouseClick -= HandleClick;
        _tokenPlacementManager.OnPlacementStarted -= OnPlacementStarted;
        _tokenPlacementManager.OnPlacementCompleted -= OnPlacementCompleted;
        _tokenPlacementManager.OnTokenPlaced -= UpdateButtonInteractability;
    }
    public void HandleTokenPlace(TokenType type) {
        _tokenPlacementManager.StartPlacingToken(type);
        UpdateButtonInteractability();
    }
    public void FinalizePlacement() {        
        _tokenPlacementManager.FinalizePlacement();
        UpdateButtonInteractability();
    }
    public void CancelPlacement() {
        _tokenPlacementManager.Cancel();
        UpdateButtonInteractability();
    }
    public void UpdatePlacement() {
        _tokenPlacementManager.UpdatePlacement(_raycastBoard);
    }
    private void OnPlacementStarted() {
        OnTokenPlacementActive?.Invoke(true);
        UpdateButtonInteractability(); 
    }

    private void OnPlacementCompleted() {
        OnTokenPlacementActive?.Invoke(false);
        UpdateButtonInteractability(); 
    }
    // private void HandleClick(Vector2 screenPosition) {
    //     Debug.Log("TokenPlacementViewModel: HandleClick called");
    //     if (/*!_raycastBoard.IsPointerOverUI() &&*/ _raycastBoard.IsMouseOverGameWindow()) {
    //         _tokenPlacementManager.TryPlace();
    //         UpdateButtonInteractability();
    //     }
    // }
    private void UpdateButtonInteractability() {
        var tracker = _tokenPlacementManager.TokenPlacementTracker;
        OnHeroButtonInteractableChanged?.Invoke(tracker.CanPlace(TokenType.Hero));
        OnHopliteButtonInteractableChanged?.Invoke(tracker.CanPlace(TokenType.HopliteStack));
        OnOkButtonInteractableChanged?.Invoke(tracker.AllPlaced());
    }
}
