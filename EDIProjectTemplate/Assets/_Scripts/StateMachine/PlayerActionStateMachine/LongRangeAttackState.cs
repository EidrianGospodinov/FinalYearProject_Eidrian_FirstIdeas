using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class LongRangeAttackState : IState<PlayerController, ActionStateId>
    {
        private float longRangeTimer;

        public ActionStateId GetId()
        {
            return ActionStateId.LongRangeAttack;
        }

        public void Enter(PlayerController playerController)
        {
            longRangeTimer = 0;
            playerController.playerAnimation.SetBoolParam("isHoldingRightMouseButton", true);
        }

        public void Update(PlayerController playerController)
        {
            if (playerController.IsWeaponEquipped || !playerController.HasRightClickHold)
            {
                playerController.ActionStateMachine.ChangeState(ActionStateId.Ready);
            }

            longRangeTimer += Time.deltaTime;
            if (longRangeTimer >= playerController.AttackData.longRangeInterval)
            {
                if (playerController.CurrentHeroData != null &&
                    playerController.EnemyDetector.CurrentActiveEnemy != null)
                {
                    EventBus<OnLongRange>.Trigger(new OnLongRange(playerController.CurrentHeroData,
                        playerController.EnemyDetector.CurrentActiveEnemy));
                    EventBus<OnAttack>.Trigger(new OnAttack(AttackType.LongRange, ComboStateId.None));
                }

                longRangeTimer = 0;
            }
        }

        public void Exit(PlayerController playerController)
        {

        }
    }
}