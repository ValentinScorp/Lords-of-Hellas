// PureEventListener<T> - base listener without MonoBehaviour
using System;

public abstract class PureEventListener<T> where T : IGameEvent
{
    private readonly Action<IGameEvent> _handlerWrapper;

    protected PureEventListener() {
        _handlerWrapper = OnEvent;
        EventBus.Listen(typeof(T), _handlerWrapper);
    }

    protected abstract void HandleEvent(T evt);

    private void OnEvent(IGameEvent evt) {
        if (evt is T typed) {
            HandleEvent(typed);
        }
    }

    public void Unsubscribe() {
        EventBus.Unlisten(typeof(T), _handlerWrapper);
    }
}
