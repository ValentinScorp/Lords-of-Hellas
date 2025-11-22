using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Oracle Bonus Draw Neutral Artifact")]
public class OracleBonusDrawNeutralArtifact : PlayerAbitilyAsset
{
    public override string Description() {
        return "Draw 3 Neutral Artifacts, keep one and shuffle other back.";
    }
    public override void Apply(Player player, Action onCompleted) {

    }
}