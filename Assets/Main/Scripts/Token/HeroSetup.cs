using UnityEngine;

internal class HeroSetup
{
    internal HeroModel.Id HeroId { get; }
    internal string DisplayName => HeroId.ToString();

    internal HeroSetup(HeroModel.Id heroId) {
        HeroId = heroId;
    }
}