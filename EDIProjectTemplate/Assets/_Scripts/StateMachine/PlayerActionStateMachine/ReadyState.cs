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
            if (agent.HasLeftClickInput)
            {
                // Consume the input FIRST
                agent.HasLeftClickInput = false; 
        
                // Set context and transition
                agent.ActionStateMachine.ChangeState(ActionStateId.Attacking);
                return;
            }
            if (agent.HasRightClickInput)
            {
                // Consume the input FIRST
                agent.HasRightClickInput = false; 
        
                //do logic right click attack                
                return;
            }
        }

        public void Exit(PlayerController agent)
        {
            
        }
    }
}