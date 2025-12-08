using System.Collections.Generic;

public class RegularAction : IGameEvent
{
    public int HeroSteps;
    public int HoplitesSteps;

    public Player Player { get; }
        
    public RegularAction(Player player) {
        Player = player;
        HeroSteps = player.Hero.Speed;
        HoplitesSteps = player.Hero.Leadership;
    }
    public IEnumerable<CardArtifact> ChargedArtifacts() {
        foreach (var artifact in Player.ArtifactCards)
            if (artifact.Charged)
                yield return artifact;
    }
    public int PriestsInPool() {
        return Player.PriestsInPool;
    }
    public int HoplitesMoveLeft() {
        return 0;
    }
}
