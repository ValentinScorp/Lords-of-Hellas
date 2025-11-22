using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardDeck
{
    private List<CardData> _deck = new();
    private List<CardData> _discardPile = new();
    public IReadOnlyList<CardData> Cards => _deck.AsReadOnly();

    public CardDeck(IEnumerable<CardData> cards) {
        _deck = cards.ToList();
        Shuffle();
    }

    public void Shuffle() {
        for (int i = 0; i < _deck.Count; i++) {
            int rand = UnityEngine.Random.Range(i, _deck.Count);
            (_deck[i], _deck[rand]) = (_deck[rand], _deck[i]);
        }
    }
    public CardData Draw() {
        if (_deck.Count == 0) {
            if (_discardPile.Count == 0) {
                Debug.LogWarning("No cards left to draw.");
                return null;
            }

            UniteDiscardWithDeck();
            Shuffle();
        }

        return DrawAndDiscard(_deck[0]);
    }
    public void UniteDiscardWithDeck() {
        _deck.AddRange(_discardPile);
        _discardPile.Clear();
    }
    public bool RemoveCard(CardData card) {
        if (card == null) return false;

        bool removedFromDeck = _deck.Remove(card);
        bool removedFromDiscard = _discardPile.Remove(card);

        return removedFromDeck || removedFromDiscard;
    }
    public CardData WatchByTitle(string title) {
        var card = _deck.FirstOrDefault(c => c.Title == title);
        if (card == null) {
            Debug.LogWarning($"Card with title '{title}' not found in deck.");
            return null;
        }
        return card;
    }
    public CardData DrawByTitle(string title) {
        return DrawAndDiscard(WatchByTitle(title));
    }    
    private CardData DrawAndDiscard(CardData card) {
        if (_deck.Remove(card)) {
            Discard(card);
            return card;
        }
        Debug.LogWarning("Card wasn't drawn because it wasn't in deck! Card Title: " + card.Title);
        return null;
    }

    public List<CardData> DrawMultiple(int count) {
        var drawn = new List<CardData>();
        for (int i = 0; i < count; i++) {
            var card = Draw();
            if (card == null) break;
            drawn.Add(card);
        }
        return drawn;
    }

    private void Discard(CardData card) {
        if (card != null) {
            _discardPile.Add(card);
        }
    }
    public void PrintDeck() {
        foreach (var card in _deck) {
            Debug.Log(card.Title);
        }
    }
    public int DeckCount => _deck.Count;
    public int DiscardCount => _discardPile.Count;
}
