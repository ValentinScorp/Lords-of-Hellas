using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class CardSelectPanel
{
    internal event Action<List<CardArtifact>> OnInitiateSelection;

    private Action OnSelectionCompleted;

    internal void SetCardSelected(CardArtifact chosenCard) {
        if (GameContext.Instance.CurrentPlayer == null) {
            Debug.LogWarning("No current player when selecting artifact card!");
            return;
        }

        GameContext.Instance.CurrentPlayer.TakeArtifactCard(chosenCard);

        if (GameContext.Instance.ArtifactDeck.RemoveCard(chosenCard)) {
            GameContext.Instance.ArtifactDeck.UniteDiscardWithDeck();
            GameContext.Instance.ArtifactDeck.Shuffle();
        } else {
            Debug.LogWarning($"Unable to remove card from deck in {this.GetType().Name}!");
        }  
    }

    internal void SubscribeToPlayers(IEnumerable<Player> players) {
        foreach (var player in players) {
            player.OnArtifactCardSelect += SelectArtifactCard;
        }
    }
    internal void ConfirmSelection() {
        OnSelectionCompleted?.Invoke();
    }
    private void SelectArtifactCard(Player player, int cardCount, Action onCompleted) {
        GameContext.Instance.CurrentPlayer = player;
        OnSelectionCompleted = onCompleted;

        List<CardArtifact> artifactCards = GameContext.Instance.ArtifactDeck
            .DrawMultiple(cardCount)
            .Cast<CardArtifact>()
            .ToList();

        // Debug.Log($"Selected {artifactCards.Count} cards");
        OnInitiateSelection?.Invoke(artifactCards);
    }

}
