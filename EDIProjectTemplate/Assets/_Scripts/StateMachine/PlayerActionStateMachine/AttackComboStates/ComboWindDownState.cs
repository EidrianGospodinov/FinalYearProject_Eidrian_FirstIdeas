using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class ComboWindDownState : IState<PlayerController,ComboStateId>
    {
        private float windDownTimer;
        public bool IsTimerDone { get; private set; }
        public ComboStateId GetId()
        {
            return ComboStateId.WindDown;
        }

        public void Enter(PlayerController agent)
        {
            agent.HasLeftClickInput = false;
            agent.HasRightClickInput = false;
            agent.IsAttacking = false;
            windDownTimer = 0.5f;
        }

        public void Update(PlayerController agent)
        {
            windDownTimer -= Time.deltaTime;
            if (windDownTimer <= 0f)
            {
                IsTimerDone = true;
            }
        }

        public void Exit(PlayerController agent)
        {
            
        }
    }
}