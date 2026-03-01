using System;

[System.Serializable]
internal class PlayerSetupConfig
{
    internal string PlayerName;
    internal HeroModel.Id HeroId;
    internal PlayerColor PlayerColor;

    internal PlayerSetupConfig() {

    }
    internal PlayerSetupConfig(string playerName = null, HeroModel.Id heroId = HeroModel.Id.None, PlayerColor color = PlayerColor.Blue) {
        PlayerName = playerName;
        HeroId = heroId;
        PlayerColor = color;
    }
}