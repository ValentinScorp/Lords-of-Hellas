using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Heracles Starting")]
public class HeraclesStartingBonus : PlayerAbitilyAsset
{
    public override string Description() {
        return "Draw 3 Neutral Artifacts and start game with one of them. Shuffle others back.";
    }
    public override void Apply(Player player, Action onCompleted) {
        // Debug.Log("Heracles starts with 2 Strength.");
        player.Hero.ChangeStrength(+1);
        onCompleted?.Invoke();
    }
}