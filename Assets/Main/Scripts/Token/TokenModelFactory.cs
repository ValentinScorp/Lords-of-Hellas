using System;
using UnityEngine;

public class TokenModelFactory
{
    public HopliteStackModel CreateHoplite(Player player) => new HopliteStackModel(player.Color);
    public HeroModel CreateHero(HeroModel.Id heroId, Player player) => new HeroModel(heroId, player);
    public Monster CreateMonster(Monster.Id monsterId) => new Monster(monsterId);
    //public TokenModel CreatePlayerToken(TokenType type, Player player) {
    //    return Create(type, player, player.Hero.HeroId);
    //}
    //public TokenModel Create(TokenType type, Player player, TokenHero.Id? heroId = null, TokenMonster.Id? monsterId = null) {
    //    return type switch {
    //        TokenType.Hoplite => CreateHoplite(player),
    //        TokenType.Hero => CreateHero(heroId ?? TokenHero.Id.None, player),
    //        TokenType.Monster => CreateMonster(monsterId ?? TokenMonster.Id.None),
    //        _ => throw new ArgumentException($"Unknown token type: {type}")
    //    };
    //}
}
