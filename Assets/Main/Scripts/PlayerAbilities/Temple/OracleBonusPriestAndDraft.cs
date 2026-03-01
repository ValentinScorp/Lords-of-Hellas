using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Add Priest and Draft")]
internal class OracleBonusPriestAndDraft : PlayerAbitilyAsset
{
    internal override string Description() {
        return "Add 1 more Priest to your Priest Pool, start a Blessing Draft.";
    }
    internal override void Apply(Player player, Action onCompleted) {

    }
}