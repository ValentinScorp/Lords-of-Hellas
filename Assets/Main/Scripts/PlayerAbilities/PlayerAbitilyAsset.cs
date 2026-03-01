using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Player Ability")]
internal abstract class PlayerAbitilyAsset : ScriptableObject
{
    internal abstract string Description();
    internal abstract void Apply(Player player, Action onCompleted);
}
