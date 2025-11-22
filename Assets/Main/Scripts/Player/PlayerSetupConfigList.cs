using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetupData", menuName = "Game/Player Setup Data")]
public class PlayerSetupConfigList : ScriptableObject
{
    public List<PlayerSetupConfig> Players = new();
}