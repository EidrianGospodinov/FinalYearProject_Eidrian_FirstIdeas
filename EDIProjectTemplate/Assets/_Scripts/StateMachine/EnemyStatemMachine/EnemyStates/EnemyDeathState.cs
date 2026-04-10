using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class EnemyDeathState : IState<AiAgent, EnemyStateId>
    {
        public EnemyStateId GetId()
        {
            return EnemyStateId.Death;
        }

        public virtual void Enter(AiAgent agent)
        {
            agent.navMeshAgent.isStopped = true;
            agent.transform.LookAt(new Vector3(agent.playerTransform.position.x, agent.transform.position.y, agent.playerTransform.position.z));
            if (agent.agentConfig.DeathStateAnimationName != "Empty")
            {
                agent.animator.CrossFadeInFixedTime(agent.agentConfig.DeathStateAnimationName, 0.2f);
            }
            
        }

        public virtual void Update(AiAgent agent)
        {
            
        }

        public virtual void Exit(AiAgent agent)
        {
        }
    }
}
