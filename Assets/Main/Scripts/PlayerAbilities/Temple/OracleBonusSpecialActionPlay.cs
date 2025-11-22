using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Additional Special Action Play")]
public class OracleBonusSpecialActionPlay : PlayerAbitilyAsset
{
    public override string Description() {
        return "You can play an additional Special Action this turn.";
    }
    public override void Apply(Player player, Action onCompleted) {
        
    }
}