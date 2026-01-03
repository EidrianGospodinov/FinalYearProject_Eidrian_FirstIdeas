using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class BasicAttackState : BaseCombatAttackState
    {
        protected override bool TryTransitionToNextState(PlayerController agent)
        {
            if (GetParentState(agent).ComboWindowTimer <= 0)
            {
                if (agent.HasLeftClickInput)
                {
                    agent.HasLeftClickInput = false;
                    GetComboSM(agent).ChangeState(ComboStateId.SecondaryBasicAttack);
                    return true; // Success! We transitioned.
                }
                else if (agent.HasRightClickInput)
                {
                    agent.HasRightClickInput = false;
                    GetComboSM(agent).ChangeState(ComboStateId.FlipAttack);
                    return true;
                }
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
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, GetId()));
            
            var comboData = data.GetComboStateId(GetId());
            parentState.ResetComboTimer(comboData.attackDelay);
        }
        

        public override void Exit(PlayerController agent)
        {
            
            base.Exit(agent);
        }
        
    }
}