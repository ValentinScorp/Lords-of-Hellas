using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Perseus Special")]
internal class PerseusSpecialAbility : PlayerAbitilyAsset
{
    internal override string Description() {
        return "After using a \"Prepare\" Special Action, place Perseus in any Region on the Map. You can't start a Quest this way.";
    }
    internal override void Apply(Player player, Action onCompleted) {
        //if (context.LastSpecialActionWasPrepare) {
        //    var targetRegion = player.ChooseAnyRegion(); // UI ����
        //    context.RegionManager.MoveToken(hero, targetRegion);
        //    // �������� ������ ������ � ����� ����� ����
        //}
    }
}