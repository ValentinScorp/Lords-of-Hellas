using UnityEngine;
using System;

public class TemplePool
{    
    private CardTemple _currentCard = null;
    private int _currentStep = 0;
    public CardTemple CurrentCard => _currentCard;

    public event Action OnTemplePoolChanged;

    public bool TrySetTempleCard(CardData card) {
        if (_currentCard != null) {
            Debug.LogWarning("Temple card already set!");
            return false;
        }

        if (card is not CardTemple templeCard) {
            Debug.LogError($"Invalid card type passed to SetTempleCard: {card.GetType().Name}");
            return false;
        }

        _currentCard = templeCard;
        _currentStep = 0;
        return true;
    }
    public void ResetTempleCard() {
        _currentCard = null;
    }
    public string CurrentStepText() {
        return _currentCard.drafts[_currentStep].ToString();
    }
    public void AdvanceStep() {
        if (_currentCard != null) {
            if (_currentStep == _currentCard.drafts.Length - 1) {
                Debug.LogWarning("Can't get temples! Out of stock!");
            } else {
                _currentStep = Mathf.Min(_currentStep + 1, _currentCard.drafts.Length - 1);
                OnTemplePoolChanged?.Invoke();
            }
        }
    }
}
