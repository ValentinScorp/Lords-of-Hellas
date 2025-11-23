using System;
using System.Collections.Generic;

public interface IGameEvent { }

public static class EventBus
{
    private static readonly Dictionary<Type, Action<IGameEvent>> _events = new();

    public static void Listen(Type eventType, Action<IGameEvent> handler) {
        if (!IsValidEventType(eventType) || handler == null) return;
        AddHandler(eventType, handler);
    }

    public static void Unlisten(Type eventType, Action<IGameEvent> handler) {
        if (!IsValidEventType(eventType) || handler == null) return;
        RemoveHandler(eventType, handler);
    }

    public static void SendEvent(IGameEvent evt) {
        if (evt == null) throw new ArgumentNullException(nameof(evt));
        var eventType = evt.GetType();
        if (_events.TryGetValue(eventType, out var del)) {
            del.Invoke(evt);
        }
    }

#if UNITY_EDITOR
    public static void ClearAll() => _events.Clear();
#endif

    private static bool IsValidEventType(Type eventType) =>
        eventType != null && typeof(IGameEvent).IsAssignableFrom(eventType);

    private static void AddHandler(Type eventType, Action<IGameEvent> handler) {
        if (_events.TryGetValue(eventType, out var existing)) {
            if (Array.Exists(existing.GetInvocationList(), d => d.Equals(handler))) return;
            _events[eventType] = (Action<IGameEvent>)Delegate.Combine(existing, handler);
        } else {
            _events[eventType] = handler;
        }
    }

    private static void RemoveHandler(Type eventType, Action<IGameEvent> handler) {
        if (_events.TryGetValue(eventType, out var existing)) {
            var updated = (Action<IGameEvent>)Delegate.Remove(existing, handler);
            if (updated == null) {
                _events.Remove(eventType);
            } else {
                _events[eventType] = updated;
            }
        }
    }
}
