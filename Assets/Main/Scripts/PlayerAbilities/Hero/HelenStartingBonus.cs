using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Helen Starting")]
public class HelenStartingBonus : PlayerAbitilyAsset
{
    public override string Description() {
        return "Draw 3 Neutral Artifacts and start game with one of them. Shuffle others back.";
    }
    public override void Apply(Player player, Action onCompleted) {
        player.SelectOneOfArtifactCards(3, onCompleted);        
    }
}