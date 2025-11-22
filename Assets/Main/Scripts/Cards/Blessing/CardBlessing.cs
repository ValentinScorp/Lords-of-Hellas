using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "NewBlessingCard", menuName = "Cards/BlessingCard")]
public class CardBlessing : ScriptableObject, CardData
{
    public string godName;
    public string titleName;
    public string effect;

    public string Title => titleName;
}
