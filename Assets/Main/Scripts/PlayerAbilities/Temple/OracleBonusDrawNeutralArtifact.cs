using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Draw Neutral Artifact")]
internal class OracleBonusDrawNeutralArtifact : PlayerAbitilyAsset
{
    internal override string Description() {
        return "Draw 3 Neutral Artifacts, keep one and shuffle other back.";
    }
    internal override void Apply(Player player, Action onCompleted) {

    }
}