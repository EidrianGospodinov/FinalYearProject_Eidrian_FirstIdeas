using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class EnemyIdleState : EnemyBaseState
    {
        private float idleTime;
        private float timeElapsed = 0f;
        public override EnemyStateId GetId()
        {
            return EnemyStateId.Idle;
        }

        public override void Enter(AiAgent agent)
        {
            base.Enter(agent);
            timeElapsed = 0f;
            agent.navMeshAgent.isStopped = true;
            idleTime = Random.Range(agent.agentConfig.minIdleTime, agent.agentConfig.maxIdleTime);
        }

        public override void Update(AiAgent agent)
        {
            base.Update(agent);
            if (agent.agentConfig.stayForeverIdle)
            {
                return;
            }
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= idleTime)
            {
                agent.stateMachine.ChangeState(EnemyStateId.Wonder); 
            }
        }

        public override void Exit(AiAgent agent)
        {
            base.Exit(agent);
        }
    }
}