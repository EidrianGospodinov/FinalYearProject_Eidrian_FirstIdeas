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
                    agent.IsPerformingAttackVisuals = false;
                    agent.stateMachine.ChangeState(EnemyStateId.CoolDown);
                }
            }
        }

        public virtual void Exit(AiAgent agent)
        {
        }
    }
}