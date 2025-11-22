using System;

[System.Serializable]
public class PlayerSetupConfig
{
    public string PlayerName;
    public Hero.Id HeroId;
    public PlayerColor PlayerColor;

    public PlayerSetupConfig() {

    }
    public PlayerSetupConfig(string playerName = null, Hero.Id heroId = Hero.Id.None, PlayerColor color = PlayerColor.Blue) {
        PlayerName = playerName;
        HeroId = heroId;
        PlayerColor = color;
    }
}