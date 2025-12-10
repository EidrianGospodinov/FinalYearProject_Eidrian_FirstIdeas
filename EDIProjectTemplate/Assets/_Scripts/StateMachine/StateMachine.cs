using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.StateMachine
{
    // T: The Agent Type (AiAgent, PlayerController)
    // TStateId: The State ID Enum
    public class StateMachine<T, TStateId> 
        where T : class 
        where TStateId : Enum
    {
        private readonly Dictionary<TStateId, IState<T, TStateId>> states 
            = new();
        
        public T agent; 
        private IState<T, TStateId> currentState;
        public TStateId CurrentStateId => currentState.GetId();
        
        public event Action<TStateId, TStateId> OnStateChanged;

        public StateMachine(T agent)
        {
            this.agent = agent ?? throw new ArgumentException(nameof(agent));
        }

        public void RegisterState(IState<T, TStateId> state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }
            TStateId id = state.GetId();
            if (!states.TryAdd(id, state))
            {
                throw new InvalidOperationException($"State {id} is already registered");
            }
            
        }

        public void Initialize(TStateId initialStateId)
        {
            if (currentState != null)
            {
                throw new InvalidOperationException("StateMachine already initialized");
            }

            currentState = GetState(initialStateId);
            currentState.Enter(agent);
        }

        public IState<T, TStateId> GetState(TStateId stateId) 
        { 
            if (!states.TryGetValue(stateId, out var state))
                throw new InvalidOperationException(
                    $"State {stateId} not registered"
                );

            return state;
        }
        
        public void Update()
        {
           if (currentState == null)
           {
               Debug.LogWarning($"State {currentState} not found for agent {agent}");
               return;
           }

           currentState.Update(agent);
        }

        public void ChangeState(TStateId newStateId)
        {
            if (currentState == null)
                throw new InvalidOperationException("StateMachine not initialized");
            var nextState = GetState(newStateId);

            if (ReferenceEquals(currentState, nextState))
                return;

            var previousState = currentState;
            currentState.Exit(agent);
            currentState = nextState;
            currentState.Enter(agent);
            OnStateChanged?.Invoke(previousState.GetId(), nextState.GetId());
            

        }
    }
}