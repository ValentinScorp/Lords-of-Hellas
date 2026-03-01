using UnityEngine;

[CreateAssetMenu(fileName = "DatabaseCardTemple", menuName = "Cards/Database Cards Temple")]
internal class CardTempleDatabase : ScriptableObject
{    
    internal CardTemple[] Cards = new CardTemple[5];
}
