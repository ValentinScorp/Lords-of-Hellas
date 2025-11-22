using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Perseus Starting")]
public class PerseusStartingBonus : PlayerAbitilyAsset
{
    public override string Description() {
        return "Take Glory Token in the same color as your starting Region.";
    }
    public override void Apply(Player player, Action onCompleted) {
        player.AddLandToken();
    }
}