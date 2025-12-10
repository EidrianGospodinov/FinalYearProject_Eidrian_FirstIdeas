namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class BasicAttackState : IState<PlayerController,ComboStateId>
    {
        public ComboStateId GetId()
        {
            return ComboStateId.BasicAttack;
        }

        public void Enter(PlayerController agent)
        {
            throw new System.NotImplementedException();
        }

        public void Update(PlayerController agent)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(PlayerController agent)
        {
            throw new System.NotImplementedException();
        }
    }
}