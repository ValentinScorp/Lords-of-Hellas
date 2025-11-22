using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSelectPanel
{
    public event Action<List<CardArtifact>> OnInitiateSelection;

    private Action OnSelectionCompleted;

    public void SetCardSelected(CardArtifact chosenCard) {
        if (GameState.Instance.CurrentPlayer == null) {
            Debug.LogWarning("No current player when selecting artifact card!");
            return;
        }

        GameState.Instance.CurrentPlayer.TakeArtifactCard(chosenCard);

        if (GameState.Instance.ArtifactDeck.RemoveCard(chosenCard)) {
            GameState.Instance.ArtifactDeck.UniteDiscardWithDeck();
            GameState.Instance.ArtifactDeck.Shuffle();
        } else {
            Debug.LogWarning($"Unable to remove card from deck in {this.GetType().Name}!");
        }  
    }

    public void SubscribeToPlayers(IEnumerable<Player> players) {
        foreach (var player in players) {
            player.OnArtifactCardSelect += SelectArtifactCard;
        }
    }
    public void ConfirmSelection() {
        OnSelectionCompleted?.Invoke();
    }
    private void SelectArtifactCard(Player player, int cardCount, Action onCompleted) {
        GameState.Instance.CurrentPlayer = player;
        OnSelectionCompleted = onCompleted;

        List<CardArtifact> artifactCards = GameState.Instance.ArtifactDeck
            .DrawMultiple(cardCount)
            .Cast<CardArtifact>()
            .ToList();

        Debug.Log($"Selected {artifactCards.Count} cards");
        OnInitiateSelection?.Invoke(artifactCards);
    }

}
