using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine
{
    public class ReadyState : IState<PlayerController, ActionStateId>
    {
        private float longRangeTimer;
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

            if (agent.HasRightClickHold)
            {
                OnLongRangeAttack(agent);
            }
            else
            {
                longRangeTimer = agent.AttackData.longRangeInterval;
            }
            if (HasDodgeInput(agent))
            {
                return;
            }
        }

        private void OnLongRangeAttack(PlayerController agent)
        {
            longRangeTimer += Time.deltaTime;
            if (longRangeTimer >= agent.AttackData.longRangeInterval)
            {
                if (agent.CurrentHeroData != null && agent.EnemyDetector.CurrentActiveEnemy != null)
                {
                    EventBus<OnLongRange>.Trigger(new OnLongRange(agent.CurrentHeroData, agent.EnemyDetector.CurrentActiveEnemy));
                }
                longRangeTimer = 0;
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
                EventBus<OnUltimate>.Trigger(new OnUltimate(agent.CurrentHeroData, agent.EnemyDetector.CurrentActiveEnemy));
            }
            if (agent.HasLeftClickInput)
            {
                agent.HasLeftClickInput = false; 
        
                agent.ActionStateMachine.ChangeState(ActionStateId.Attacking);
                return true;
            }
            if (agent.HasRightClickInput)
            {
                //we are performing the action inside the attack state, but we need to indicate that there is input
                agent.HasRightClickInput = false; 
        
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