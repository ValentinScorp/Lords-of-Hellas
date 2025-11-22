using System.Collections.Generic;
using UnityEngine;

public class TempleCardSelectSceneBootstrap : MonoBehaviour
{
    private CardLoader _cardLoader;
    private CardDeck _eventDeck;

    private void Start() {
        _cardLoader = new CardLoader();
        _cardLoader.LoadEventCards();

        List<CardData> eventCards = new List<CardData>();
        eventCards.AddRange(_cardLoader.MonsterCards);
        eventCards.AddRange(_cardLoader.QuestCards);
        _eventDeck = new CardDeck(eventCards);
    }
}