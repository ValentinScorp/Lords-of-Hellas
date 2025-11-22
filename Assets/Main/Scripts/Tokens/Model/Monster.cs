using UnityEngine;

public class Monster : Token
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
