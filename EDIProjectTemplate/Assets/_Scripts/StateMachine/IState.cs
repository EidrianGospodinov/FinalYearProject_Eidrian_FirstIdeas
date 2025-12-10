using System;

namespace _Scripts.StateMachine
{
    public interface IState<T, TStateId> where T : class where TStateId : Enum
    {
        TStateId GetId(); // Returns its specific enum type
        void Enter(T agent);
        void Update(T agent);
        void Exit(T agent);
    }
}