using _Scripts.Units.Enemy;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public abstract class EnemyAttackBaseState: IState<AiAgent, EnemyStateId>
    {
        public abstract EnemyStateId GetId();

        public virtual void Enter(AiAgent agent)
        {
        }

        public virtual void Update(AiAgent agent)
        {
            
            if (agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
            {
                agent.stateMachine.ChangeState(EnemyStateId.ReadyToAttack);
            }
        }

        public virtual void Exit(AiAgent agent)
        {
        }
    }
}