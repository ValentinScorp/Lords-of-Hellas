using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetupData", menuName = "Game/Player Setup Data")]
internal class PlayerSetupConfigList : ScriptableObject
{
    internal List<PlayerSetupConfig> Players = new();
}