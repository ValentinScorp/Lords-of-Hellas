using UnityEngine;

public abstract class EventListener<T> : MonoBehaviour where T : IGameEvent
{
    protected virtual void Awake() {
        EventBus.Listen<T>(OnEvent);
    }
    protected virtual void OnDestroy() {
        EventBus.Unlisten<T>(OnEvent);
    }
    private void OnEvent(T evt) {
        Debug.Log("event got!");
        HandleEvent(evt); 
    }
    protected abstract void HandleEvent(T evt);
}
