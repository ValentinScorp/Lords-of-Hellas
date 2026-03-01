using System;
using UnityEngine;

[System.Serializable]
internal class HeroModel : TokenModel
{
    internal enum Id {
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

    internal Id HeroId { get; }
    internal int Leadership { get; private set; }
    internal int Speed { get; private set; }
    internal int Strength { get; private set; }

    internal string DisplayName => HeroId.ToString();
    internal event Action<int> LeadershipChanged;
    internal event Action<int> SpeedChanged;
    internal event Action<int> StrengthChanged;

    internal HeroModel(Id heroId, Player player) 
        : base (TokenType.Hero, player.Color) {
        HeroId = heroId;
        player.Hero = this;

        _config = HeroDatabase.GetConfig(heroId);
        ChangeStrength(_config.BaseStrength);
        ChangeSpeed(_config.BaseSpeed);
        ChangeLeadership(_config.BaseLeadership);        
    }

    internal void ChangeStrength(int delta) {
        Strength = Mathf.Max(0, Strength + delta);
        StrengthChanged?.Invoke(Strength);
    }
    internal void ChangeSpeed(int delta) {
        Speed = Mathf.Max(0, Speed + delta);
        SpeedChanged?.Invoke(Speed);
    }
    internal void ChangeLeadership(int delta) {
        Leadership = Mathf.Max(0, Leadership + delta);
        LeadershipChanged?.Invoke(Leadership);
    }
    internal void ApplyStartinBonus(Player player, Action onCompleted) {
         if (_config == null || _config.StartingBonus == null) {
            Debug.LogError($"Missing StartingBonus for hero {HeroId}");
            onCompleted?.Invoke();
            return;
        }
        _config.StartingBonus.Apply(player, onCompleted);
    }

    internal void RefreshStats()
    {
        StrengthChanged?.Invoke(Strength);
        SpeedChanged?.Invoke(Speed);
        LeadershipChanged?.Invoke(Leadership);
    }
}

