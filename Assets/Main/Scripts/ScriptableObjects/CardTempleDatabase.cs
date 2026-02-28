using UnityEngine;

[CreateAssetMenu(fileName = "DatabaseCardTemple", menuName = "Cards/Database Cards Temple")]
public class CardTempleDatabase : ScriptableObject
{    
    public CardTemple[] Cards = new CardTemple[5];
}
