using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class BasicAttackState : IState<PlayerController,ComboStateId>
    {
        public ComboStateId GetId()
        {
            return ComboStateId.BasicAttack;
        }

        public void Enter(PlayerController agent)
        {
            Debug.Log("basic attack enter");
            AttackData data = agent.AttackData;
            var parentState = (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, ComboStateId.BasicAttack));
            
            
            parentState.ResetComboTimer(data.attackDelay);
        }

        public void Update(PlayerController agent)
        {
            
        }

        public void Exit(PlayerController agent)
        {
            
        }
        
        private AttackingState GetParentState(PlayerController agent)
        {
            // We retrieve the AttackingState instance from the top-level machine
            return (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
        }
    }
}