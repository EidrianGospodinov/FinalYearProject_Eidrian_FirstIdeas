public interface IEventBus<T>
{
    void Register(IEventBinding<T> binding);
    void Unregister(IEventBinding<T> binding);
    void Trigger(T e);
}