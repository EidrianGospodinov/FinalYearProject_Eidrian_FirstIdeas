using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public abstract class EnemyAttackBaseState: IState<AiAgent, EnemyStateId>
    {
        private float performAnimTime;
        public abstract EnemyStateId GetId();

        public virtual void Enter(AiAgent agent)
        {
            agent.navMeshAgent.isStopped = false;
            performAnimTime = 0;
            agent.AttackHasLanded = false;
        }

        public virtual void Update(AiAgent agent)
        {
            if (agent.IsPerformingAttackVisuals)
            {
                performAnimTime += Time.deltaTime;
                if (performAnimTime >= agent.NextAttackTypeData.animationDuration)
                {
                    Debug.Log("IsPerforming attack visuals false in the attack base state update before going back to cooldown");

                    agent.IsPerformingAttackVisuals = false;
                    agent.stateMachine.ChangeState(EnemyStateId.CoolDown);
                }
            }
            else
            {
                if (!agent.IsPlayerDetected(true))
                {
                    agent.stateMachine.ChangeState(EnemyStateId.CoolDown);
                }
            }
        }

        public virtual void Exit(AiAgent agent)
        {
        }
    }
}