using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventCardSelectionMenu : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _cardListContainer;
    [SerializeField] private Transform _cardSelectedListContainer;
    [SerializeField] private CardQuestView _questCardPrefab;
    [SerializeField] private CardMonsterView _monsterCardPrefab;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _nextButton;

    private List<CardData> _eventCards = new List<CardData>();
    private List<CardData> _eventCardsSelected = new List<CardData>();
    private CardView _currentlySelectedCard;

    private void Start() {
        _selectButton.onClick.AddListener(SelectCard);

        _nextButton.onClick.AddListener(NextScene);
        _nextButton.interactable = false;

        if (CardLoader.Instance.LoadEventCards(out var _eventCards)) {
            foreach(var card in _eventCards) {
                if (card is CardQuest quest) {
                    AddQuestCard(quest);
                }
                if (card is CardMonster monster) {
                    AddMonsterCard(monster);
                }
            }
        } else { 
            Debug.Log("Can't load event cards!");
        }
    }
    private void AddQuestCard(CardQuest questCard) {
        CardQuestView card = Instantiate(_questCardPrefab, _cardListContainer);
        if (card == null) {
            Debug.LogError("Can't instantiate CardQuestView prefab!");
            Destroy(questCard);
        } else {
            _eventCards.Add(questCard);
            card.Init(questCard, OnCardSelected);
        }
    }
    private void AddMonsterCard(CardMonster monsterCard) {
        CardMonsterView card = Instantiate(_monsterCardPrefab, _cardListContainer);
        if (card == null) {
            Debug.LogError("Can't instantiate CardMonsterView prefab!");
            Destroy(monsterCard);
        } else {
            _eventCards.Add(monsterCard);
            card.Init(monsterCard, OnCardSelected);
        }
    }
    private void OnCardSelected(CardView selectedCard) {
        _currentlySelectedCard = selectedCard;
    }
    private void SelectCard() {
        if (_currentlySelectedCard == null) {
            Debug.LogWarning("No card selected!");
            return;
        }
        _currentlySelectedCard.transform.SetParent(_cardSelectedListContainer, false);


        if (!_eventCardsSelected.Contains(_currentlySelectedCard.Data)) {
            _eventCardsSelected.Add(_currentlySelectedCard.Data);
        }

        _eventCards.Remove(_currentlySelectedCard.Data);

        _currentlySelectedCard = null;
        UpdateNextButton();
        UpdateSelectButton();
    }
    private void UpdateSelectButton() {        
        if (_eventCardsSelected.Count < 7) {
            _selectButton.interactable = true;
        } else {
            _selectButton.interactable = false;
        }
    }
    private void UpdateNextButton() {
        if (_eventCardsSelected.Count != 7) {
            _nextButton.interactable = false;
            return;
        }
        _nextButton.interactable = true;
    }
    private void NextScene() {
        List<string> selectedCardsTitles = new List<string>();
        foreach (var card in _eventCardsSelected) {
            GameConfig.Instance.AddEventCard(card);
        }
        
        SceneManager.LoadScene("03_PlayerSetupMenu");
    }   
}
