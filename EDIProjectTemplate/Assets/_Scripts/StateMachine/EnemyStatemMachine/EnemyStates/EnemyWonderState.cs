using _Scripts.Units.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
     public class EnemyWonderState : IState<AiAgent, EnemyStateId>
    {
        private float timeSinceLastDestination = 0f;

        public void Enter(AiAgent agent)
        {
            timeSinceLastDestination = 0f;
            agent.navMeshAgent.isStopped = false;
            SetNewRandomDestination(agent);
        }
        

        public void Update(AiAgent agent)
        {
            if (agent.IsPlayerDetected())
            {
               // agent.stateMachine.ChangeState(AiStateId.Chase);
            }
            timeSinceLastDestination += Time.deltaTime;
        
            // If the agent has stopped moving
            if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
            {
                agent.stateMachine.ChangeState(EnemyStateId.Idle);
            }
            else if (timeSinceLastDestination >= agent.agentConfig.destinationRefreshTime)
            {
                SetNewRandomDestination(agent);
            }
        }

        public void Exit(AiAgent agent)
        {
            agent.navMeshAgent.isStopped = true;
        }

       

        
        private void SetNewRandomDestination(AiAgent agent)
        {
            timeSinceLastDestination = 0f;
            Vector3 offset = GetRandomPointInRing(agent.agentConfig.minWanderRadius, agent.agentConfig.wanderRadius);
        
            // Use agent current position to get a position close to him
            Vector3 randomPosition = offset + agent.transform.position; 
            //Debug.Log(offset);

            NavMeshHit hit;
            
            if (NavMesh.SamplePosition(randomPosition, out hit, agent.agentConfig.wanderRadius, NavMesh.AllAreas))
            {
                agent.navMeshAgent.SetDestination(hit.position); 
            }
        }

        private Vector3 GetRandomPointInRing(float minRadius, float maxRadius)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0;
            randomDirection.Normalize();

            //Get a random distance between the min and max radius
            float randomDistance = Random.Range(minRadius, maxRadius);
            return randomDirection * randomDistance;
        }

        EnemyStateId IState<AiAgent, EnemyStateId>.GetId()
        {
            return EnemyStateId.Wonder;
        }
    }
}