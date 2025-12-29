using UnityEngine;

public class HeroSetup
{
    public HeroModel.Id HeroId { get; }
    public string DisplayName => HeroId.ToString();

    public HeroSetup(HeroModel.Id heroId) {
        HeroId = heroId;
    }
}