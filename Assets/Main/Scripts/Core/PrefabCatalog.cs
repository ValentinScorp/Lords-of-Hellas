using UnityEngine;

[CreateAssetMenu(fileName = "PrefabCatalog", menuName = "Game/PrefabCatalog")]
public class PrefabCatalog : ScriptableObject
{
    [SerializeField] private TempleVisual _templePrefab;

    public TempleVisual TemplePrefab => _templePrefab;
}