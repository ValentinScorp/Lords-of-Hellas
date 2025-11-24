using UnityEngine;

public class Monster : TokenEntity
{
    public enum Id
    {
        None,
        Cerberus,
        Cyclops,
        Hydra,
        Medusa,
        Minotaur,
        Sphinx
    }
    public Id MonsterId { get; }
    public Monster(Id monsterId) 
        : base(TokenType.Monster) {
        MonsterId = monsterId;
    }
}
