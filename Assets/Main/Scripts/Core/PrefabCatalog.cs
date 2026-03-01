using UnityEngine;

[CreateAssetMenu(fileName = "PrefabCatalog", menuName = "Game/PrefabCatalog")]
internal class PrefabCatalog : ScriptableObject
{
    [SerializeField] private TempleVisual _templePrefab;
    [SerializeField] private HopliteIconView _hopliteIconPrefab;

    internal TempleVisual TemplePrefab => _templePrefab;
}