using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Attributes Raise")]
public class OracleBonusAttributesRise : PlayerAbitilyAsset
{
    public override string Description() {
        return "Permanently raise 2 different Attributes by 1.";
    }
    public override void Apply(Player player, Action onCompleted) {
        
    }
}