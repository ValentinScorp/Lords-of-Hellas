using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Draw Combat Cards")]
public class OracleBonusDrawCombatCards : PlayerAbitilyAsset
{
    public override string Description() {
        return "Draw Combat Cards up to your Combat Cards limit.";
    }
    public override void Apply(Player player, Action onCompleted) {

    }
}