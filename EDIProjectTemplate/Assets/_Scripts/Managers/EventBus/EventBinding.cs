using System;

  /// <summary>
    /// Interface defining the contract for an event binding.  An event binding
    /// associates an event type with the actions (methods) that should be executed
    /// when that event is triggered.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
public interface IEventBinding<T>
{
    /// <summary>
    /// Delegate representing a method that takes one argument of type T (the event type)
    /// and returns void.  This is used to store the event handlers that should be
    /// executed when the event is triggered.
    /// </summary>
    public Action<T> OnEvent { get; set; }

    /// <summary>
    /// Delegate representing a method that takes no arguments and returns void.  This
    /// is used to store event handlers that don't need the event data itself.
    /// </summary>
    public Action onEventNoArgs { get; set; }
}
public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
     /// <summary>
        /// Private field to store the Action<T> delegate.  Initialized with an empty lambda
        /// expression to prevent null reference exceptions.
        /// </summary>
        Action<T> onEvent = _ => { };

        /// <summary>
        /// Private field to store the Action delegate (no arguments). Initialized with an empty
        /// lambda expression to prevent null reference exceptions.
        /// </summary>
        Action onEventNoArgs = () => { };

        /// <summary>
        /// Explicit interface implementation of the OnEvent property from the IEventBinding<T>
        /// interface.  Provides the getter and setter for the onEvent field.
        /// </summary>
        Action<T> IEventBinding<T>.OnEvent
        {
            get => onEvent;
            set => onEvent = value;
        }

        /// <summary>
        /// Explicit interface implementation of the onEventNoArgs property from the
        /// IEventBinding<T> interface.  Provides the getter and setter for the onEventNoArgs field.
        /// </summary>
        Action IEventBinding<T>.onEventNoArgs
        {
            get => onEventNoArgs;
            set => onEventNoArgs = value;
        }

        /// <summary>
        /// Constructor that takes an Action<T> delegate as a parameter and sets the onEvent
        /// field to the value passed in the constructor.  This allows you to initialize the
        /// event binding with a specific event handler.
        /// </summary>
        /// <param name="onEvent">The event handler to be executed.</param>
        public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;

        /// <summary>
        /// Constructor that takes an Action delegate as a parameter and sets the onEventNoArgs
        /// field to the value passed in the constructor.  This allows you to initialize the
        /// event binding with a specific event handler that takes no arguments.
        ///
        /// NOTE: There was a bug in the original code where this constructor was assigning
        /// this.onEventNoArgs to itself.  This has been corrected.
        /// </summary>
        /// <param name="onEventNoArgs">The event handler to be executed.</param>
        public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;

        /// <summary>
        /// Method that allows you to add an event handler to the onEvent delegate.  It uses
        /// the += operator to combine the existing delegate with the new delegate. This
        /// supports multiple listeners for the same event.
        /// </summary>
        /// <param name="onEvent">The event handler to add.</param>
        public void AddListener(Action<T> onEvent) => this.onEvent += onEvent;

        /// <summary>
        /// Method that allows you to remove an event handler from the onEvent delegate. It uses
        /// the -= operator to remove the delegate.
        /// </summary>
        /// <param name="onEvent">The event handler to remove.</param>
        public void RemoveListener(Action<T> onEvent) => this.onEvent -= onEvent;

        /// <summary>
        /// Method that allows you to add an event handler to the onEventNoArgs delegate.
        /// </summary>
        /// <param name="onEvent">The event handler to add.</param>
        public void AddListener(Action onEvent) => onEventNoArgs += onEvent;

        /// <summary>
        /// Method that allows you to remove an event handler from the onEventNoArgs delegate.
        /// </summary>
        /// <param name="onEvent">The event handler to remove.</param>
        public void RemoveListener(Action onEvent) => onEventNoArgs -= onEvent;

}