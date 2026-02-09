using System;
using System.Collections.Generic;
using UnityEngine;

public static class SceneUiRegistry
{
    private static readonly Dictionary<Type, object> _views = new();
    public static void Register<T>(T instance) where T : class => _views[typeof(T)] = instance;
    public static void Unregister<T>() where T : class => _views.Remove(typeof(T));
    public static T Get<T>() where T : class
    {
        T view = _views.TryGetValue(typeof(T), out var s) ? (T)s : null;
        if (view == null) {
            Debug.LogWarning($"Unable to get view {typeof(T).Name} from SceneViewRegistry!");
        }
        return view;
    }
    public static void LogRegisteredServices()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Registered views:");

        foreach (var kv in _views) {
            var type = kv.Key;
            var instance = kv.Value;
            sb.AppendLine($"- {type.FullName} => {(instance != null ? instance.GetType().FullName : "null")}");
        }

        Debug.Log(sb.ToString());
    }
}