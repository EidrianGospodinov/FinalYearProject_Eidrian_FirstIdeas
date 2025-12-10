using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public abstract class BaseCombatAttackState : IState<PlayerController,ComboStateId>
    {
        protected AttackingState GetParentState(PlayerController agent)
        {
            return (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
        }
    
        protected StateMachine<PlayerController, ComboStateId> GetComboSM(PlayerController agent)
        {
            return GetParentState(agent).GetComboStateMachine;
        }
        protected abstract bool TryTransitionToNextState(PlayerController agent);
        public abstract ComboStateId GetId();

        public virtual void Enter(PlayerController agent)
        {
            
        }

        public virtual void Update(PlayerController agent)
        {
            if (TryTransitionToNextState(agent))
            {
                return;
            }
            
            if (GetParentState(agent).ComboWindowTimer <= 0)
            {
                GetComboSM(agent).ChangeState(ComboStateId.WindDown);
            }
        }

        public virtual void Exit(PlayerController agent)
        {
        }
    }
}