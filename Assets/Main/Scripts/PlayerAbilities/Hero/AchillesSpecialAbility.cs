using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Achilles Special")]
internal class AchillesSpecialAbility : PlayerAbitilyAsset
{
    internal override string Description() {
        return "Add Achilles' Speed or Strength (whichever is lowest) to your Army Strenth in the Region he is in.";
    }
    internal override void Apply(Player player, Action onCompleted) {
        //var region = context.RegionManager.GetRegionData(hero.CurrentRegion);
        //int bonus = Mathf.Min(hero.Speed, hero.Strength);
        //region.ModifyArmyStrength(player.Color, +bonus);
    }
}