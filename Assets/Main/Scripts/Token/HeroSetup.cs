using UnityEngine;

public class HeroSetup
{
    public Hero.Id HeroId { get; }
    public string DisplayName => HeroId.ToString();

    public HeroSetup(Hero.Id heroId) {
        HeroId = heroId;
    }
}