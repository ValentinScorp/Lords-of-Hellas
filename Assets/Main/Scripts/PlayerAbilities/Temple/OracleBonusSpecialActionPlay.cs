using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Additional Special Action Play")]
internal class OracleBonusSpecialActionPlay : PlayerAbitilyAsset
{
    internal override string Description() {
        return "You can play an additional Special Action this turn.";
    }
    internal override void Apply(Player player, Action onCompleted) {
        
    }
}