using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class SecondBasicAttack : BaseCombatAttackState
    {
        protected override bool TryTransitionToNextState(PlayerController agent)
        {
            if(GetParentState(agent).ComboWindowTimer <= 0)
            {
                if (agent.HasLeftClickInput)
                {
                    agent.HasLeftClickInput = false;
                    GetComboSM(agent).ChangeState(ComboStateId.SpecialAttack);
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
            return ComboStateId.SecondaryBasicAttack;
        }

        public override void Enter(PlayerController playerController)
        {
            base.Enter(playerController);
            Debug.Log("secondary attack enter");
            AttackData data = playerController.AttackData;
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, GetId()));
            
            var parentState = (AttackingState)playerController.ActionStateMachine.GetState(ActionStateId.Attacking);
            var comboData = data.GetComboStateId(GetId());
            parentState.ResetComboTimer(comboData.attackDelay);
        }

        

        public override void Exit(PlayerController agent)
        {
        }
    }
}