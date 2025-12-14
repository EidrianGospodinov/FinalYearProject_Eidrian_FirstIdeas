using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class BasicAttackState : BaseCombatAttackState
    {
        protected override bool TryTransitionToNextState(PlayerController agent)
        {
            if (agent.HasLeftClickInput && GetParentState(agent).ComboWindowTimer <= 0)
            {
                agent.HasLeftClickInput = false; 
                GetComboSM(agent).ChangeState(ComboStateId.SecondaryBasicAttack); 
                return true; // Success! We transitioned.
            }

            return false;
        }

        public override ComboStateId GetId()
        {
            return ComboStateId.BasicAttack;
        }

        public override void Enter(PlayerController agent)
        {
            base.Enter(agent);
            Debug.Log("basic attack enter");
            AttackData data = agent.AttackData;
            var parentState = (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, ComboStateId.BasicAttack));
            
            parentState.ResetComboTimer(data.attackDelay);
        }
        

        public override void Exit(PlayerController agent)
        {
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.NONE, ComboStateId.WindDown));
            base.Exit(agent);
        }
        
        private AttackingState GetParentState(PlayerController agent)
        {
            // We retrieve the AttackingState instance from the top-level machine
            return (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
        }
    }
}