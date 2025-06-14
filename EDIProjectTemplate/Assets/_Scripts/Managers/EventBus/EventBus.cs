using System;
using System.Collections.Generic;

/// <summary>
/// Marker interface for all event types.  Any class or struct implementing this
/// is considered an event that can be used with the EventBus.
/// </summary>
public interface IEvent{ }

/// <summary>
/// Implementation of the EventBus  Provides methods for registering,
/// unregistering, and triggering events.
/// </summary>
public static class EventBus<T> where T : IEvent
{
    /// <summary>
    /// Hashset to store the event bindings.
    /// </summary>
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

    /// <summary>
    /// Registers an event binding with the event bus.
    /// </summary>
    /// <param name="binding">The event binding to register.</param>
    public static void Register(IEventBinding<T> binding) => bindings.Add(binding);

    /// <summary>
    /// Unregisters an event binding from the event bus.
    /// </summary>
    /// <param name="binding">The event binding to unregister.</param>
    public static void Unregister(IEventBinding<T> binding) => bindings.Remove(binding);

    /// <summary>
    /// Triggers an event, causing all registered event handlers to be executed.
    /// </summary>
    /// <param name="event">The event object to trigger.</param>
    public static void Trigger(T @event)
    {
        foreach (var binding in bindings)
        {
            binding.OnEvent.Invoke(@event);
            binding.onEventNoArgs.Invoke();
        }
    }
}

