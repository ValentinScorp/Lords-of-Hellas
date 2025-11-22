using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Player Ability")]
public abstract class PlayerAbitilyAsset : ScriptableObject
{
    public abstract string Description();
    public abstract void Apply(Player player, Action onCompleted);
}
