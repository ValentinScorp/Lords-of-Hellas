using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Attributes Raise")]
internal class OracleBonusAttributesRise : PlayerAbitilyAsset
{
    internal override string Description() {
        return "Permanently raise 2 different Attributes by 1.";
    }
    internal override void Apply(Player player, Action onCompleted) {
        
    }
}