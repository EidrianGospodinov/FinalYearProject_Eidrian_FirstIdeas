using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class DashingState : IState<PlayerController, ActionStateId>
    {
        private float dodgeEndTime;
        private const float DODGE_DURATION = 0.5f;
        public ActionStateId GetId()
        {
            return ActionStateId.Dashing;
        }

        public void Enter(PlayerController agent)
        {
            Debug.Log("Entered dodging state");
            dodgeEndTime = Time.time + DODGE_DURATION;
            agent.SetDodgeCooldown();
            agent.PerformDashMovement(DODGE_DURATION);
            //agent.PlayAnimation("Dodge");
        }

        public void Update(PlayerController agent)
        {
            agent.CharacterController.Move(agent.DashVelocity * Time.deltaTime);
            if (Time.time >= dodgeEndTime)
            {
                agent.ActionStateMachine.ChangeState(ActionStateId.Ready);
            }
        }

        public void Exit(PlayerController agent)
        {
            
        }
    }
}