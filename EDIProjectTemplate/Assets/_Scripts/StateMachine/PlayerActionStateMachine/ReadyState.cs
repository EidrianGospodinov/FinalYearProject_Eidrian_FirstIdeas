using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class ReadyState : IState<PlayerController, ActionStateId>
    {
        public ActionStateId GetId()
        {
            return ActionStateId.Ready;
        }

        public void Enter(PlayerController playerController)
        {
            Debug.Log("enter ready state");
        }

        public void Update(PlayerController playerController)
        {
            if (playerController.IsWeaponEquipped && HasAttackingInput(playerController))
            {
                return;
            }

            if (!playerController.IsWeaponEquipped && playerController.HasRightClickHold)
            {
                playerController.ActionStateMachine.ChangeState(ActionStateId.LongRangeAttack);
                return;
            }
            if (HasDodgeInput(playerController))
            {
                return;
            }
        }


        public void Exit(PlayerController playerController)
        {
            
        }

        private bool HasAttackingInput(PlayerController playerController)
        {
            if (playerController.HasSpecialPowerInput)
            {
                playerController.HasSpecialPowerInput = false;
                Debug.LogError("Do Special attack");
                EventBus<OnUltimate>.Trigger(new OnUltimate(playerController.CurrentHeroData, playerController.EnemyDetector.CurrentActiveEnemy));
            }
            if (playerController.HasLeftClickInput)
            {
                playerController.HasLeftClickInput = false; 
        
                playerController.ActionStateMachine.ChangeState(ActionStateId.Attacking);
                return true;
            }
            if (playerController.HasRightClickInput)
            {
                //we are performing the action inside the attack state, but we need to indicate that there is input
                playerController.HasRightClickInput = false; 
        
                return true;
            }

            return false;
        }
        private bool HasDodgeInput(PlayerController playerController)
        {
            if (playerController.HasDashInput)
            {
                playerController.HasDashInput = false;
                playerController.ActionStateMachine.ChangeState(ActionStateId.Dashing);
                return true;
            }

            return false;
        }
    }
}