
using System;

public class HeroViewModel : TokenViewModel
{
    public int Leadership { get; private set; }
    public int Speed { get; private set; }
    public int Strength { get; private set; }

    public HeroViewModel(HeroModel hero, TokenView view) : base(hero, view)
    {
        if (hero == null || view == null) return;

        Leadership = hero.Leadership;
        Speed = hero.Speed;
        Strength = hero.Strength;

        hero.OnLeadershpChanged += view.HandleLeadershipChanged;
        hero.OnSpeedChanged += view.HandleSpeedChanged;
        hero.OnStrengthChanged += view.HandleStrengthChanged;
    }
    public void Unbind()
    {
        if (Model == null || View == null) return;

        if (Model is HeroModel hero) {
            hero.OnLeadershpChanged -= View.HandleLeadershipChanged;
            hero.OnSpeedChanged -= View.HandleSpeedChanged;
            hero.OnStrengthChanged -= View.HandleStrengthChanged;
        }
    }

    public override void Dispose()
    {
        Unbind();
        base.Dispose();
    }
}
