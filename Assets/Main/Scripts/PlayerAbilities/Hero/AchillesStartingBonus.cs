using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Achilles Starting")]
internal class AchillesStartingBonus : PlayerAbitilyAsset
{
    internal override string Description() {
        return "Achilles starts with 2 Speed.";
    }
    internal override void Apply(Player player, Action onCompleted) {
        player.Hero.ChangeSpeed(+1);
        player.Hero.ChangeLeadership(+1);
        onCompleted?.Invoke();
    }
}