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

        public void Enter(PlayerController playerController)
        {
            playerController.HasLeftClickInput = false;
            playerController.HasRightClickInput = false;
            playerController.IsAttacking = false;
            windDownTimer = 0.5f;
        }

        public void Update(PlayerController playerController)
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