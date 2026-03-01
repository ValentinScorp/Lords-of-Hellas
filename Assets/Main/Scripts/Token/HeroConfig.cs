using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroConfig", menuName = "Game/Create Hero Config")]
internal class HeroConfig : ScriptableObject
{
    internal int BaseLeadership = 1;
    internal int BaseSpeed = 1;
    internal int BaseStrength = 1;
    
    [SerializeField] private PlayerAbitilyAsset _startingBonus;
    internal PlayerAbitilyAsset StartingBonus => _startingBonus;
    [SerializeField] private PlayerAbitilyAsset _specialAbility;
    internal PlayerAbitilyAsset SpecialAbility => _specialAbility;

    internal string DisplayName;

}
