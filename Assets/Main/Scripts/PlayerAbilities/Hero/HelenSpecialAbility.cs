using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Helen Special")]
public class HelenSpecialAbility: PlayerAbitilyAsset
{
    public override string Description() {
        return "Heracles starts with 2 Strength.";
    }
    public override void Apply(Player player, Action onCompleted) {
        // Логіка: вороги не можуть заходити хоплітами, якщо немає їхнього героя
        //context.RegionManager.SetRegionEntryRule(hero.CurrentRegion, (enteringPlayer, enteringToken) => {
        //    if (enteringPlayer == player) return true;
        //    if (enteringToken.TokenType != TokenType.Hoplite) return true;

        //    var region = context.RegionManager.GetRegionData(hero.CurrentRegion);
        //    return region.Tokens.Any(t => t is Hero h && h.Player == enteringPlayer);
        //});
    }
}