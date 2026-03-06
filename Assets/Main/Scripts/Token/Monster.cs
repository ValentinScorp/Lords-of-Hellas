using UnityEngine;

internal class Monster : TokenModel
{
    internal enum Id
    {
        None,
        Cerberus,
        Cyclops,
        Hydra,
        Medusa,
        Minotaur,
        Sphinx
    }
    internal Id MonsterId { get; }
    internal Monster(Id monsterId) 
        : base(TokenType.Monster, PlayerColor.Grey) {
        MonsterId = monsterId;
    }
}
