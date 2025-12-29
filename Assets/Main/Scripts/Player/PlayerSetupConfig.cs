using System;

[System.Serializable]
public class PlayerSetupConfig
{
    public string PlayerName;
    public HeroModel.Id HeroId;
    public PlayerColor PlayerColor;

    public PlayerSetupConfig() {

    }
    public PlayerSetupConfig(string playerName = null, HeroModel.Id heroId = HeroModel.Id.None, PlayerColor color = PlayerColor.Blue) {
        PlayerName = playerName;
        HeroId = heroId;
        PlayerColor = color;
    }
}