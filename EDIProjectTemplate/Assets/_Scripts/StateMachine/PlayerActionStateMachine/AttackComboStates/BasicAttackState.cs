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
        }

        public void Update(PlayerController agent)
        {
            
        }

        public void Exit(PlayerController agent)
        {
            
        }
    }
}