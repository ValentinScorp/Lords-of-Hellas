using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Helen Special")]
internal class HelenSpecialAbility: PlayerAbitilyAsset
{
    internal override string Description() {
        return "Heracles starts with 2 Strength.";
    }
    internal override void Apply(Player player, Action onCompleted) {
        // �����: ������ �� ������ �������� ��������, ���� ���� ������� �����
        //context.RegionManager.SetRegionEntryRule(hero.CurrentRegion, (enteringPlayer, enteringToken) => {
        //    if (enteringPlayer == player) return true;
        //    if (enteringToken.TokenType != TokenType.Hoplite) return true;

        //    var region = context.RegionManager.GetRegionData(hero.CurrentRegion);
        //    return region.Tokens.Any(t => t is Hero h && h.Player == enteringPlayer);
        //});
    }
}