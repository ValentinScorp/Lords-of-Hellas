// PureEventListener<T> Ч базовий клас без MB
public abstract class PureEventListener<T> where T : IGameEvent
{
    protected PureEventListener() {
        EventBus.Listen<T>(HandleEvent);
    }

    protected abstract void HandleEvent(T evt);

    public void Unsubscribe() {
        EventBus.Unlisten<T>(HandleEvent);
    }
}