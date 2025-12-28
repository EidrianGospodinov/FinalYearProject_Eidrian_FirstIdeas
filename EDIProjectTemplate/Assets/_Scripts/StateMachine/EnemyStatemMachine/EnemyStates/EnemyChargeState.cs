using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class EnemyChargeState : EnemyAttackBaseState
    {
        private Vector3 ChargeDestination;
        private float previousSpeed;
        float previousAcceleration;
        public override EnemyStateId GetId()
        {
            return EnemyStateId.Charge;
        }

        public override void Enter(AiAgent agent)
        {
            base.Enter(agent);
            agent.navMeshAgent.stoppingDistance = 0;
            previousSpeed = agent.navMeshAgent.speed;
            agent.navMeshAgent.speed = 18;
            previousAcceleration = agent.navMeshAgent.acceleration;
            agent.navMeshAgent.acceleration = 10;
            SetOvershootDestination(agent,6);
        }

        public override void Update(AiAgent agent)
        {
            base.Update(agent);
        }

        public override void Exit(AiAgent agent)
        {
            agent.navMeshAgent.speed = previousSpeed;
            agent.navMeshAgent.acceleration = previousAcceleration;
        }
        public void SetOvershootDestination(AiAgent agent, float extraMeters)
        {
            ChargeDestination = agent.playerTransform.transform.position;
            
            Vector3 currentPos = agent.transform.position;
    
            //Get the direction from AI to Player
            Vector3 direction = (ChargeDestination - currentPos).normalized;
            
            Vector3 overshootPos = ChargeDestination + (direction * extraMeters);
    
            agent.navMeshAgent.SetDestination(overshootPos);
        }
    }
}