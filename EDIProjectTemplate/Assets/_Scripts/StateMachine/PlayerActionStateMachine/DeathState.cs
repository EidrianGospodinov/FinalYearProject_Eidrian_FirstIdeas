using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class DeathState : IState<PlayerController, ActionStateId>
    {
        private float _timer;
        private const float DelayTime = 3.0f;
        private bool _hasTriggered;
        
        public ActionStateId GetId()
        {
            return ActionStateId.Death;
        }

        public void Enter(PlayerController playerController)
        {
            _timer = 0;
            _hasTriggered = false;
            
            playerController.SetDeathState();

            
            playerController.RespawnService.ShowMenu(playerController);
            /*playerController.RespawnService.Respawn(playerController);

            playerController.ActionStateMachine.ChangeState(ActionStateId.Ready);*/
        }

        public void Update(PlayerController playerController)
        {
            if (_hasTriggered) return;

            _timer += Time.deltaTime;

            if (_timer >= DelayTime)
            {
                // Now show the UI after the delay
                playerController.RespawnService.ShowMenu(playerController);
                _hasTriggered = true;
            }
        }

        public void Exit(PlayerController playerController)
        {
        }
    }
}