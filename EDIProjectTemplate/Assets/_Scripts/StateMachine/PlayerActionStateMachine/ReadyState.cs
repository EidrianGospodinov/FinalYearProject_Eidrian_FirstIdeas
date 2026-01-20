using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class ReadyState : IState<PlayerController, ActionStateId>
    {
        public ActionStateId GetId()
        {
            return ActionStateId.Ready;
        }

        public void Enter(PlayerController agent)
        {
            Debug.Log("enter ready state");
        }

        public void Update(PlayerController agent)
        {
            if (agent.IsWeaponEquipped && HasAttackingInput(agent))
            {
                return;
            }

            if (HasDodgeInput(agent))
            {
                return;
            }
        }
        

        public void Exit(PlayerController agent)
        {
            
        }

        private bool HasAttackingInput(PlayerController agent)
        {
            if (agent.HasSpecialPowerInput)
            {
                agent.HasSpecialPowerInput = false;
                Debug.LogError("Do Special attack");
                
            }
            if (agent.HasLeftClickInput)
            {
                // Consume the input FIRST
                agent.HasLeftClickInput = false; 
        
                // Set context and transition
                agent.ActionStateMachine.ChangeState(ActionStateId.Attacking);
                return true;
            }
            if (agent.HasRightClickInput)
            {
                // Consume the input FIRST
                agent.HasRightClickInput = false; 
        
                //do logic right click attack                
                return true;
            }

            return false;
        }
        private bool HasDodgeInput(PlayerController agent)
        {
            if (agent.HasDashInput)
            {
                agent.HasDashInput = false;
                agent.ActionStateMachine.ChangeState(ActionStateId.Dashing);
                return true;
            }

            return false;
        }
    }
}