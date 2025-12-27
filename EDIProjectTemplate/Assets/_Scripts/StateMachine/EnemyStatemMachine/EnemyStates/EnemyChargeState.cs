using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class EnemyChargeState : EnemyAttackBaseState
    {
        private Vector3 ChargeDestination;
        public override EnemyStateId GetId()
        {
            return EnemyStateId.Charge;
        }

        public override void Enter(AiAgent agent)
        {
            base.Enter(agent);
            agent.navMeshAgent.stoppingDistance = 0;
            ChargeDestination = agent.playerTransform.transform.position;
            agent.navMeshAgent.SetDestination(ChargeDestination);
        }

        public override void Update(AiAgent agent)
        {
            base.Update(agent);
        }

        public override void Exit(AiAgent agent)
        {
        }
    }
}