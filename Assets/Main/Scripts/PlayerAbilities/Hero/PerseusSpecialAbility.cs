using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Perseus Special")]
public class PerseusSpecialAbility : PlayerAbitilyAsset
{
    public override string Description() {
        return "After using a \"Prepare\" Special Action, place Perseus in any Region on the Map. You can't start a Quest this way.";
    }
    public override void Apply(Player player, Action onCompleted) {
        //if (context.LastSpecialActionWasPrepare) {
        //    var targetRegion = player.ChooseAnyRegion(); // UI вибір
        //    context.RegionManager.MoveToken(hero, targetRegion);
        //    // Заборона старту квесту — можна через флаг
        //}
    }
}