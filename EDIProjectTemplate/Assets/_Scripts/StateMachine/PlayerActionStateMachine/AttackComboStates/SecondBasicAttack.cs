using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class SecondBasicAttack : BaseCombatAttackState
    {
        protected override bool TryTransitionToNextState(PlayerController agent)
        {
            if (agent.HasLeftClickInput && GetParentState(agent).ComboWindowTimer <= 0)
            {
                agent.HasLeftClickInput = false; 
                //GetComboSM.ChangeState(ComboStateId.); 
                return true; // Success! We transitioned.
            }

            return false;
        }

        public override ComboStateId GetId()
        {
            return ComboStateId.SecondaryBasicAttack;
        }

        public override void Enter(PlayerController agent)
        {
            Debug.Log("secondary attack enter");
            AttackData data = agent.AttackData;
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, ComboStateId.SecondaryBasicAttack));
            
            var parentState = (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
            parentState.ResetComboTimer(data.attackDelay);
        }

        

        public override void Exit(PlayerController agent)
        {
        }
    }
}