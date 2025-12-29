using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class AttackGeneric : EnemyAttackBaseState
    {
        private float pathUpdateDeadline;
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
                if (Vector3.Distance(agent.navMeshAgent.destination, agent.playerTransform.position) >= 0.2f)
                {
                    pathUpdateDeadline = Time.time + Random.Range(0f, 0.1f);
                    agent.navMeshAgent.SetDestination(agent.playerTransform.position);
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