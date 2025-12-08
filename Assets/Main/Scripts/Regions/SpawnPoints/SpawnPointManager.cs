using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] GameObject _spawnPointPrefab;
    private const float _spawnDistance = 1f;
    private void Start() {
        for (int i = 0; i < 5; i++) {
            Vector3 position = new Vector3(i * _spawnDistance, 0, 0);
        }
    }
    private GameObject SpawnAt(Vector3 position, Quaternion? rotation = null, Transform parent = null) {
        if (_spawnPointPrefab == null) {
            Debug.LogError("SpawnPointDisplay: _spawnPointPrefab is not assigned!");
            return null;
        }

        Quaternion spawnRotation = rotation ?? Quaternion.identity;
        GameObject spawn = Instantiate(_spawnPointPrefab, position, spawnRotation, parent);

        return spawn;
    }
}
