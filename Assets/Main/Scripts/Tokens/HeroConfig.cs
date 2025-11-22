using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroConfig", menuName = "Game/Create Hero Config")]
public class HeroConfig : ScriptableObject
{
    public int BaseLeadership = 1;
    public int BaseSpeed = 1;
    public int BaseStrength = 1;

    public PlayerAbitilyAsset StartingBonus;
    public PlayerAbitilyAsset SpecialAbility;

    public string DisplayName;

}
