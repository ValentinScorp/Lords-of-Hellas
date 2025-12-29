using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempleCardSelectionPanel : MonoBehaviour
{
    [SerializeField] private Transform _cardListContainer;
    [SerializeField] private CardTempleView _cardPrefab;
    [SerializeField] private CardTempleDatabase _cardDatabase;
    [SerializeField] private Button _nextButton;

    private List<CardTempleView> _cardDisplayList = new List<CardTempleView>();
    private CardTempleView _selectedCard = null;

    private void Start() {
        _nextButton.interactable = false;
        _nextButton.onClick.AddListener(NextScene);

        foreach (var card in _cardDatabase.Cards) {
            var cardDisplay = Instantiate(_cardPrefab, _cardListContainer);
            cardDisplay.Init(card, OnCardSelected);
            _cardDisplayList.Add(cardDisplay);
        }
    }
    private void OnCardSelected(CardTemple card) {
        if (_selectedCard) {
            Destroy(_selectedCard.gameObject);
        }
        _selectedCard = Instantiate(_cardPrefab, transform);
        _selectedCard.Init(card);
        GameConfig.Instance.TempleCard = card;
        _nextButton.interactable = true;
    }
    private void NextScene() {
        SceneManager.LoadScene("02_EventCardSelection");
    }
}
