using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegularAction
{
    public int HeroSteps;
    public int HoplitesSteps;

    public class HopliteMoveInfo
    {
        public HopliteModel Hoplite;
        public bool Moved;
    }
    public List<HopliteMoveInfo> HoplitesMoveList = new();

    public Player Player { get; }

    public RegularAction(Player player)
    {
        Player = player;
        HeroSteps = player.Hero.Speed;
        HoplitesSteps = player.Hero.Leadership;
        MakeHoplitesMoveList();
    }
    public IEnumerable<CardArtifact> ChargedArtifacts()
    {
        foreach (var artifact in Player.ArtifactCards)
            if (artifact.Charged)
                yield return artifact;
    }
    public int PriestsInPool()
    {
        return Player.PriestsInPool;
    }
    public int HoplitesMoveLeft()
    {
        if (HoplitesMoveList.Count == 0) {
            return 0;
        }
        int notMoved = HoplitesMoveList.Count(h => !h.Moved);
        return Mathf.Min(HoplitesSteps, notMoved);
    }
    private void MakeHoplitesMoveList()
    {
        var hopliteStacks = GameContext.Instance.RegionDataRegistry.GetHopliteStacks(Player.Color);

        foreach(var stack in hopliteStacks) {
            foreach (var hoplite in stack.Hoplites) {
                HoplitesMoveList.Add(new HopliteMoveInfo {
                    Hoplite = hoplite,
                    Moved = false
                });
            }
        }
       
    }
    public bool TryTakeUnmovedHoplite(HopliteStackModel hopliteStack, out HopliteModel unmovedHoplite)
    {
        unmovedHoplite = null;
        if (hopliteStack == null) {
            return false;
        }
        foreach (var hoplite in hopliteStack.Hoplites) {
            var moveInfo = HoplitesMoveList.FirstOrDefault(h => h.Hoplite == hoplite);
            if (moveInfo != null && !moveInfo.Moved) {
                // moveInfo.Moved = true;
                unmovedHoplite = hoplite;
                return true;
            }
        }

        return false;
    }
}
