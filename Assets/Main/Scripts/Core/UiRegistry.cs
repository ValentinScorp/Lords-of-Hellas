using System;
using System.Collections.Generic;
using UnityEngine;
public class UiRegistry
{
    private readonly List<MonoBehaviour> _views = new();

    public void Register(MonoBehaviour view)
    {
        if (IsExist(view)) {
            _views.Add(view);
        }
    }
    public T Get<T>() where T : MonoBehaviour
    {
        foreach (var view in _views)
        {
            if (view is T typed) return typed;
        }
        Debug.LogWarning($"UiRegistry: {typeof(T).Name} not found");
        return null;
    }
    private bool IsExist(object obj) {
        if (obj == null) {
            Debug.LogWarning($"Tring to register null in UiRegistry!");
            return false;
        }
        return true;
    }
}
