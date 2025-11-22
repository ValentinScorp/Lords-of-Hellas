using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGameEvent { }

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> _events = new();

    public static void Listen<T>(Action<T> handler) {
        if (handler == null) return;
        var type = typeof(T);
        _events[type] = Delegate.Combine(_events.GetValueOrDefault(type), handler);
    }

    public static void Unlisten<T>(Action<T> handler) {
        if (handler == null) return;
        var type = typeof(T);
        if (_events.TryGetValue(type, out var del)) {
            _events[type] = Delegate.Remove(del, handler);
        }
    }

    public static void SendEvent<T>(T evt) {
        Debug.Log("Sending event!" + evt);
        Debug.Log(_events.Count);

        if (_events.TryGetValue(typeof(T), out var del)) {
            (del as Action<T>)?.Invoke(evt);
        }
    }

    // Для тестів
#if UNITY_EDITOR
    public static void ClearAll() => _events.Clear();
#endif
}