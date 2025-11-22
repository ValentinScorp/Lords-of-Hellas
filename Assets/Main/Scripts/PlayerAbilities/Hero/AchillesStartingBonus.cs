using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Achilles Starting")]
public class AchillesStartingBonus : PlayerAbitilyAsset
{
    public override string Description() {
        return "Achilles starts with 2 Speed.";
    }
    public override void Apply(Player player, Action onCompleted) {
        player.Hero.ChangeSpeed(+1);
    }
}