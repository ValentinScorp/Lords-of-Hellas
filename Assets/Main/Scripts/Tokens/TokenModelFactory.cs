using System;
using UnityEngine;

public class TokenModelFactory
{
    public HopliteStack CreateHoplite(Player player) => new HopliteStack(player);
    public Hero CreateHero(Hero.Id heroId, Player player) => new Hero(heroId, player);
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
