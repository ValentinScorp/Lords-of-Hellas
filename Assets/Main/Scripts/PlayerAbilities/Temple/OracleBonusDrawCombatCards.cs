using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Draw Combat Cards")]
internal class OracleBonusDrawCombatCards : PlayerAbitilyAsset
{
    internal override string Description() {
        return "Draw Combat Cards up to your Combat Cards limit.";
    }
    internal override void Apply(Player player, Action onCompleted) {

    }
}