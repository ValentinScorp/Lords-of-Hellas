using UnityEngine;

public class Monster : TokenModel
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
        : base(TokenType.Monster, PlayerColor.Gray) {
        MonsterId = monsterId;
    }
}
