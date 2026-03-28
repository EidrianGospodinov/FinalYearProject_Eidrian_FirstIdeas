using _Scripts.Units.Enemy;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class AttackGeneric : EnemyAttackBaseState
    {
        private float pathUpdateDeadline;
        private Vector3 lastPlayerPosition;
        public override EnemyStateId GetId()
        {
            return EnemyStateId.AttackGeneric;
        }

        public override void Enter(AiAgent agent)
        {
            base.Enter(agent);
            //AttackChoice attack = agent.PendingAttack;
            pathUpdateDeadline = 0;
            agent.navMeshAgent.stoppingDistance = agent.agentConfig.attackRange;
        }

        public override void Update(AiAgent agent)
        {
            base.Update(agent);
            if (agent.IsPerformingAttackVisuals) return;
            

            
            // Only recalculate path a few times per second to save CPU
            if (Time.time >= pathUpdateDeadline)
            {
                float playerMoved = Vector3.Distance(lastPlayerPosition, agent.playerTransform.position);
                if (playerMoved >= 0.5f)
                {
                    agent.navMeshAgent.SetDestination(agent.playerTransform.position);
                    lastPlayerPosition = agent.playerTransform.position;
                    
                    pathUpdateDeadline = Time.time + Random.Range(0.3f, 0.5f);
                }
            }
            if (!agent.navMeshAgent.pathPending && 
                agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
            {
                ExecuteAttackVisuals(agent);
                return;
            }
        }

        public override void Exit(AiAgent agent)
        {
            base.Exit(agent);
        }
        private void ExecuteAttackVisuals(AiAgent agent)
        {
            agent.IsPerformingAttackVisuals = true;
    
            // Stop the agent immediately so the animation is grounded
            agent.navMeshAgent.isStopped = true;
            agent.navMeshAgent.velocity = Vector3.zero; 
    
            // Snap rotation to face the player one last time before the hit
            agent.transform.LookAt(new Vector3(agent.playerTransform.position.x, agent.transform.position.y, agent.playerTransform.position.z));

            agent.animator.CrossFadeInFixedTime(agent.NextAttackTypeData.AnimationName, 0.2f);
        }
    }
}