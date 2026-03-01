using System.Collections.Generic;
using UnityEngine;

internal static class HeroDatabase
{
    private static readonly Dictionary<HeroModel.Id, HeroConfig> _cache = new();

    internal static HeroConfig GetConfig(HeroModel.Id id) {
        if (_cache.TryGetValue(id, out var config))
            return config;

        string path = $"ScriptableObjects/Heroes/{id}";
        config = Resources.Load<HeroConfig>(path);

        if (config == null)
            Debug.LogError($"[HeroDatabase] Hero not found: {path}");

        _cache[id] = config;
        return config;
    }
}