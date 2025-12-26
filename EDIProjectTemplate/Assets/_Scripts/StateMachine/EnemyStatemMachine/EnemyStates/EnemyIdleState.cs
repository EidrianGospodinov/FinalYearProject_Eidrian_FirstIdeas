using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class EnemyIdleState : IState<AiAgent, EnemyStateId>
    {
        private float idleTime;
        private float timeElapsed = 0f;
        public EnemyStateId GetId()
        {
            return EnemyStateId.Idle;
        }

        public void Enter(AiAgent agent)
        {
            timeElapsed = 0f;
            agent.navMeshAgent.isStopped = true;
            idleTime = Random.Range(agent.agentConfig.minIdleTime, agent.agentConfig.maxIdleTime);
        }

        public void Update(AiAgent agent)
        {
            if (agent.agentConfig.stayForeverIdle)
            {
                return;
            }
            if (agent.IsPlayerDetected())
            {
                //agent.stateMachine.ChangeState(EnemyStateId.Chase);
            }
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= idleTime)
            {
                agent.stateMachine.ChangeState(EnemyStateId.Wonder); 
            }
        }

        public void Exit(AiAgent agent)
        {
            
        }
    }
}