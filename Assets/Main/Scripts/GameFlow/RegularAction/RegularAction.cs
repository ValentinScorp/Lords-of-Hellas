using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegularAction : IGameEvent
{
    public int HeroSteps;
    public int HoplitesSteps;

    public class HopliteMoveInfo
    {
        public HopliteUnit Hoplite;
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
        var registry = ServiceLocator.Get<RegionDataRegistry>();
        foreach (var region in GameState.Instance.RegionStatuses) {
            var data = registry.GetRegionData(region.RegionId);
            if (data == null) continue;

            foreach (var token in data.Tokens) {
                if (token is HopliteStack stack && stack.PlayerColor == Player.Color) {
                    foreach (var hoplite in stack.GetHoplites()) {
                        HoplitesMoveList.Add(new HopliteMoveInfo {
                            Hoplite = hoplite,
                            Moved = false
                        });
                    }
                }
            }
        }
    }
}
