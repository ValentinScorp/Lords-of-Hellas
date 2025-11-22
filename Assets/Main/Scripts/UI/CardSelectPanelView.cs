using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectPanelView : MonoBehaviour
{
    [SerializeField] private RectTransform _cardsContainer; 
    [SerializeField] private CardArtifactView _artifactCardPrefab;
    [SerializeField] private Button _confirmButton;

    private CardSelectPanel _cardSelectPanel;
    private CardArtifactId _cardSelected = CardArtifactId.Unknown;
    private CardArtifact _selectedCard;

    public void Initialize(CardSelectPanel selectPanel) {
        gameObject.SetActive(false);
        _cardSelectPanel = selectPanel;
        _confirmButton.onClick.AddListener(ConfirmSelection);
        _cardSelectPanel.OnInitiateSelection += StartCardSelection;
    }
    public void StartCardSelection(List<CardArtifact> cards) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
            _confirmButton.interactable = false;
            foreach (CardArtifact card in cards) {
                AddCard(card);
            }
        } else {
            Debug.LogError("Can't add new cards. CardSelectPanelView already active!");
        }
    }    
    public void SelectCard(CardView card) {
        _confirmButton.interactable = true;
        if (card.Data is CardArtifact artifactData) {
            _selectedCard = artifactData;
        }
    }
    private void ConfirmSelection() {      

        if (_selectedCard != null) {
            _cardSelectPanel.SetCardSelected(_selectedCard);
            _confirmButton.interactable = false;            
        } else {
            Debug.LogWarning($"Card with ID {_cardSelected} not found among displayed cards.");
        }
        gameObject.SetActive(false);
        _cardSelectPanel.ConfirmSelection();
    }
    private void AddCard(CardArtifact card) {
        CardView cardView = Instantiate(_artifactCardPrefab, _cardsContainer);
        if (cardView != null) {
            cardView.Init(card, SelectCard);
        } else {
            Debug.LogError("Can't find CardArtifactView component in Artifact Card Prefab!");
            Destroy(cardView);
        }
    }
}
