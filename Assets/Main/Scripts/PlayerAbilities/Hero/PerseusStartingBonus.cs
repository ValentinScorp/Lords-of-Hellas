using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Perseus Starting")]
internal class PerseusStartingBonus : PlayerAbitilyAsset
{
    internal override string Description() {
        return "Take Glory Token in the same color as your starting Region.";
    }
    internal override void Apply(Player player, Action onCompleted) {
        player.AddLandToken();
        onCompleted?.Invoke();
    }
}