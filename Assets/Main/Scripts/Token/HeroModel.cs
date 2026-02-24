using System;
using UnityEngine;

[System.Serializable]
public class HeroModel : TokenModel
{
    public enum Id {
        None,
        Heracles,
        Achilles,
        Perseus,
        Helen,
        Hektor,
        Odysseus,
        Cassandra,
        Leonidas,
        Cleito
    }
    private HeroConfig _config;

    public Id HeroId { get; }
    public int Leadership { get; private set; }
    public int Speed { get; private set; }
    public int Strength { get; private set; }

    public string DisplayName => HeroId.ToString();
    public event Action<int> LeadershipChanged;
    public event Action<int> SpeedChanged;
    public event Action<int> StrengthChanged;

    public HeroModel(Id heroId, Player player) 
        : base (TokenType.Hero, player) {
        HeroId = heroId;
        player.Hero = this;

        _config = HeroDatabase.GetConfig(heroId);
        ChangeStrength(_config.BaseStrength);
        ChangeSpeed(_config.BaseSpeed);
        ChangeLeadership(_config.BaseLeadership);        
    }

    public void ChangeStrength(int delta) {
        Strength = Mathf.Max(0, Strength + delta);
        StrengthChanged?.Invoke(Strength);
    }
    public void ChangeSpeed(int delta) {
        Speed = Mathf.Max(0, Speed + delta);
        SpeedChanged?.Invoke(Speed);
    }
    public void ChangeLeadership(int delta) {
        Leadership = Mathf.Max(0, Leadership + delta);
        LeadershipChanged?.Invoke(Leadership);
    }
    public void ApplyStartinBonus(Player player, Action onCompleted) {
        _config.StartingBonus.Apply(player, onCompleted);
    }

    internal void RefreshStats()
    {
        StrengthChanged?.Invoke(Strength);
        SpeedChanged?.Invoke(Speed);
        LeadershipChanged?.Invoke(Leadership);
    }
}

