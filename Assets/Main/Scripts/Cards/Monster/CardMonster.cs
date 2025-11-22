using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[CreateAssetMenu(fileName = "NewMonsterCard", menuName = "Cards/MonsterCard")]
public class CardMonster : ScriptableObject, CardData
{
    public string monster;
    public string region;
    public string evolveText;
    public string[] evolveHits;
    public string Title => monster;
    public string Region => region;
    public string Evolve => evolveText;
}
