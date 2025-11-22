using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Add Priest and Draft")]
public class OracleBonusPriestAndDraft : PlayerAbitilyAsset
{
    public override string Description() {
        return "Add 1 more Priest to your Priest Pool, start a Blessing Draft.";
    }
    public override void Apply(Player player, Action onCompleted) {

    }
}