using System;
using System.Collections.Generic;
using UnityEngine;

public class TokenLoader
{
    private readonly Dictionary<TokenType, GameObject> _prefabCache = new();
    private readonly Dictionary<TokenType, float> _radiusCache = new();

    public TokenLoader(string tokenPrefabFolder) {
        var allPrefabs = Resources.LoadAll<GameObject>(tokenPrefabFolder);
        foreach (var prefab in allPrefabs) {
            if (Enum.TryParse<TokenType>(prefab.name.Replace("Token_", ""), out var type)) {
                _prefabCache[type] = prefab;

                var collider = prefab.GetComponent<SphereCollider>();
                if (collider != null) {
                    _radiusCache[type] = collider.radius;
                } else {
                    _radiusCache[type] = 0.5f; // fallback
                    Debug.LogWarning($"TokenLoader: Prefab '{prefab.name}' does not have a SphereCollider. Using default radius 0.5.");
                }
            } else {
                Debug.LogWarning($"TokenLoader: Could not parse TokenType from prefab name '{prefab.name}'.");
            }
        }
    }
    public TokenLoader(IEnumerable<TokenType> supportedTypes) {
        LoadPrefabs(supportedTypes);
    }

    private void LoadPrefabs(IEnumerable<TokenType> types) {
        foreach (var type in types) {
            string path = $"Main/Prefabs/Token_{type}";
            GameObject prefab = Resources.Load<GameObject>(path);

            if (prefab == null) {
                Debug.LogError($"[TokenLoader] Prefab not found at path: {path}");
                continue;
            }

            _prefabCache[type] = prefab;

            var collider = prefab.GetComponent<SphereCollider>();
            _radiusCache[type] = collider != null ? collider.radius : 0.5f;
        }
    }

    public GameObject GetPrefab(TokenType type) {
        return _prefabCache.TryGetValue(type, out var prefab) ? prefab : null;
    }

    public float GetRadius(TokenType type) {
        return _radiusCache.TryGetValue(type, out var radius) ? radius : 0.5f;
    }
}
