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
                GetComboSM(agent).ChangeState(ComboStateId.SpecialAttack);
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
            base.Enter(agent);
            Debug.Log("secondary attack enter");
            AttackData data = agent.AttackData;
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, GetId()));
            
            var parentState = (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
            var comboData = data.GetComboStateId(GetId());
            parentState.ResetComboTimer(comboData.attackDelay);
        }

        

        public override void Exit(PlayerController agent)
        {
        }
    }
}