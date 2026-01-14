
using System;

public class HeroViewModel : TokenViewModel
{
    public string DisplayName => (Model as HeroModel)?.DisplayName ?? "Unknown Hero";
    public event Action<int> LeadershipChanged;
    public event Action<int> SpeedChanged;
    public event Action<int> StrengthChanged;

     public HeroViewModel(HeroModel hero) : base(hero)
    {
        LeadershipChanged?.Invoke(hero.Leadership);
        SpeedChanged?.Invoke(hero.Speed);
        StrengthChanged?.Invoke(hero.Strength);

        hero.OnLeadershpChanged += HandleLeadershipChanged;
        hero.OnSpeedChanged += HandleSpeedChanged;
        hero.OnStrengthChanged += HandleStrengthChanged;
    }

    private void HandleLeadershipChanged(int value) => LeadershipChanged?.Invoke(value);
    private void HandleSpeedChanged(int value) => SpeedChanged?.Invoke(value);
    private void HandleStrengthChanged(int value) => StrengthChanged?.Invoke(value);
    public void RefreshStats()
    {
        if (Model is HeroModel hero) {
            LeadershipChanged?.Invoke(hero.Leadership);
            SpeedChanged?.Invoke(hero.Speed);
            StrengthChanged?.Invoke(hero.Strength);
        }
    }
    public override void Dispose()
    {
        if (Model is HeroModel hero) {
            hero.OnLeadershpChanged -= HandleLeadershipChanged;
            hero.OnSpeedChanged -= HandleSpeedChanged;
            hero.OnStrengthChanged -= HandleStrengthChanged;
        }
        base.Dispose();
    }
}
