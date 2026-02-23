using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;


public class SpecialActionRecruit
{
    internal void Launch(Player player, Action<Player> OnRecruitCompleted)
    {
        var regions = GameContext.Instance.RegionRegistry;
        var controlledRegionIds = GameContext.Instance.RegionRegistry.GetControlled(player);
        if (HoplitesToPlace(controlledRegionIds) > player.HoplitesInHand) {
            // TODO: Select regions to place panel
            OnRecruitCompleted?.Invoke(player);
        } else {
            foreach (var regId in controlledRegionIds) {
                PlaceHoplite(player, regions, regId);
                PlaceHoplite(player, regions, regId);
                if (regId == RegionId.Laconia) {
                    PlaceHoplite(player, regions, regId);
                    PlaceHoplite(player, regions, regId);
                }
            }
            OnRecruitCompleted?.Invoke(player);
        }        
    }
    private void PlaceHoplite(Player player, RegionsContext regions, RegionId regionId)
    {
        if (player.TryTakeHoplite(out var hoplite)) {
            regions.TryPlace(hoplite, regionId);
        } else {
            Debug.LogWarning($"Unable to take hoplite from player {player.Color}!");
        }
    }
    private int HoplitesToPlace(List<RegionId> regions)
    {
        if (regions == null || regions.Count == 0) {
            return 0;
        }
        int hoplitesToPlace = regions.Count * 2;
        if (regions.Contains(RegionId.Laconia)) {
            hoplitesToPlace += 2;
        }
        return hoplitesToPlace;
    }
}
