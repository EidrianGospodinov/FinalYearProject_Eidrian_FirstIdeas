using _Scripts.Units.Enemy;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public abstract class EnemyAttackBaseState: IState<AiAgent, EnemyStateId>
    {
        public abstract EnemyStateId GetId();

        public virtual void Enter(AiAgent agent)
        {
            agent.navMeshAgent.isStopped = false;
        }

        public virtual void Update(AiAgent agent)
        {
            
            if (!agent.navMeshAgent.pathPending && agent.navMeshAgent.remainingDistance <= agent.navMeshAgent.stoppingDistance)
            {
                agent.stateMachine.ChangeState(EnemyStateId.CoolDown);
            }
        }

        public virtual void Exit(AiAgent agent)
        {
        }
    }
}