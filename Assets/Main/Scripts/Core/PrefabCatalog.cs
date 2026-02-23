using UnityEngine;

[CreateAssetMenu(fileName = "PrefabCatalog", menuName = "Game/PrefabCatalog")]
public class PrefabCatalog : ScriptableObject
{
    [SerializeField] private TempleView _templePrefab;

    public TempleView TemplePrefab => _templePrefab;
}