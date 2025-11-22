using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterAttackCard", menuName = "Cards/MonsterAttackCard")]
public class CardMonsterAttack : ScriptableObject, CardData
{
    public string attackName;
    public string value;
    public string effect;

    public string Title => attackName;
}
