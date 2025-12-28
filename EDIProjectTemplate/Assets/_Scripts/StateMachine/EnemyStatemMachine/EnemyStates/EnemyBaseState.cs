using _Scripts.Units.Enemy;

namespace _Scripts.StateMachine.EnemyStatemMachine.EnemyStates
{
    public abstract class EnemyBaseState : IState<AiAgent, EnemyStateId>
    {
        public abstract EnemyStateId GetId();

        public virtual void Enter(AiAgent agent)
        {
        }

        public virtual void Update(AiAgent agent)
        {
            if (agent.IsPlayerDetected(true))
            {
                agent.stateMachine.ChangeState(EnemyStateId.ReadyToAttack);
            }
        }

        public virtual void Exit(AiAgent agent)
        {
        }
    }
}