using System;
using _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class AttackingState :  IState<PlayerController, ActionStateId>
    {
        private StateMachine<PlayerController, ComboStateId> comboStateMachine;
        public ActionStateId GetId()
        {
            return ActionStateId.Attacking;
        }

        public void Enter(PlayerController agent)
        {
            Debug.Log("Attack state enter");
            comboStateMachine = new StateMachine<PlayerController, ComboStateId>(agent);
            
            comboStateMachine.RegisterState(new BasicAttackState());
            
            comboStateMachine.Initialize(ComboStateId.BasicAttack);
        }

        public void Update(PlayerController agent)
        {
            comboStateMachine.Update();
            if (comboStateMachine.CurrentStateId == ComboStateId.WindDown)
            {
                var windDownState = comboStateMachine.GetState(ComboStateId.WindDown);
                var actualWindDownState = (ComboWindDownState)windDownState;
                if (actualWindDownState.IsTimerDone)
                {
                    agent.ActionStateMachine.ChangeState(ActionStateId.Ready);
                }
            }
        }

        public void Exit(PlayerController agent)
        {
            
        }
    }
}