using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Heracles Special")]
public class HeraclesSpecialAbility : PlayerAbitilyAsset
{
    public override string Description() {
        return "At the beginning of your turn you can remove one Glory Token to draw 2 Neutral Artifacts, keep one and shuffle the others back.";
    }
    public override void Apply(Player player, Action onCompleted) {
        //if (player.GloryTokens > 0) {
        //    player.SpendGlory(1);
        //    var artifacts = context.ArtifactDeck.DrawNeutral(2);
        //    var chosen = player.ChooseArtifact(artifacts);
        //    player.AddArtifact(chosen);
        //    context.ArtifactDeck.ShuffleBack(new[] { artifacts[0] == chosen ? artifacts[1] : artifacts[0] });
        //}
    }
}