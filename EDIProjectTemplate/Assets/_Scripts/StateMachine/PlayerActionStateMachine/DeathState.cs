using Unity.VisualScripting;
using Zenject;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class DeathState : IState<PlayerController, ActionStateId>
    {
        public ActionStateId GetId()
        {
            return ActionStateId.Death;
        }

        public void Enter(PlayerController playerController)
        {
            /*playerController.RespawnService.Respawn(playerController);
            
            playerController.ActionStateMachine.ChangeState(ActionStateId.Ready);*/
        }

        public void Update(PlayerController playerController)
        {
        }

        public void Exit(PlayerController playerController)
        {
        }
    }
}