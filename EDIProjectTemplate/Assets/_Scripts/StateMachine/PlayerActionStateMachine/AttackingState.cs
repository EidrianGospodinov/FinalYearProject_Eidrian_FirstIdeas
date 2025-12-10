using System;
using _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates;

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
            comboStateMachine = new StateMachine<PlayerController, ComboStateId>(agent);
            
            comboStateMachine.RegisterState(new BasicAttackState());
            
            comboStateMachine.Initialize(ComboStateId.BasicAttack);
        }

        public void Update(PlayerController agent)
        {
            comboStateMachine.Update();
            
        }

        public void Exit(PlayerController agent)
        {
            
        }
    }
}