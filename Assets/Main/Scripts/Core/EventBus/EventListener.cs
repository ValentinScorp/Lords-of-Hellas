using System;
using UnityEngine;

public abstract class EventListener : MonoBehaviour
{
    protected abstract Type EventType { get; }

    protected virtual void Awake() {
        EventBus.Listen(EventType, OnEvent);
    }

    protected virtual void OnDestroy() {
        EventBus.Unlisten(EventType, OnEvent);
    }

    private void OnEvent(IGameEvent evt) {
        HandleEvent(evt);
    }

    protected abstract void HandleEvent(IGameEvent evt);
}
