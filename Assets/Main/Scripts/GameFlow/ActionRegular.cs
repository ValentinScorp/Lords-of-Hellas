using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ActionRegular : IGameEvent
{
    public int HeroSteps;
    public int HoplitesSteps;

    public Player Player { get; }
    public event System.Action OnComplete;
        
    public ActionRegular(Player player) {
        Player = player;
        HeroSteps = player.Hero.Speed;
        HoplitesSteps = player.Hero.Leadership;
    }
    public void Start() {
        EventBus.SendEvent(this);
    }
    public void Complete() {
        OnComplete?.Invoke();
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
