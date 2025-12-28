using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public class AttackWindDownState : IState<AiAgent, EnemyStateId>
    {
        private float windDownTimer;
        public bool IsTimerDone { get; private set; }

        public EnemyStateId GetId()
        {
            return EnemyStateId.WindDown;
        }

        public void Enter(AiAgent agent)
        {
            windDownTimer = 2f;
            agent.navMeshAgent.isStopped = true;
            /*DecideNextAttack(agent);*/
        }

        public void Update(AiAgent agent)
        {
            windDownTimer -= Time.deltaTime;
            if (windDownTimer <= 0f)
            {
                IsTimerDone = true;
                agent.stateMachine.ChangeState(EnemyStateId.ReadyToAttack);
            }
        }

        public void Exit(AiAgent agent)
        {
            
        }
    }
}