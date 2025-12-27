using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class EnemyChargeState : IState<AiAgent, EnemyStateId>
    {
        private Vector3 ChargeDestination;
        public EnemyStateId GetId()
        {
            return EnemyStateId.Charge;
        }

        public void Enter(AiAgent agent)
        {
            agent.navMeshAgent.stoppingDistance = 0;
            ChargeDestination = agent.playerTransform.transform.position;
            agent.navMeshAgent.SetDestination(ChargeDestination);
        }

        public void Update(AiAgent agent)
        {
            
        }

        public void Exit(AiAgent agent)
        {
        }
    }
}