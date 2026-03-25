using _Scripts.Units.Player;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class SecondaryFlipAttackState : BaseCombatAttackState
    {
        protected override bool TryTransitionToNextState(PlayerController agent)
        {
            if (agent.HasLeftClickInput && GetParentState(agent).ComboWindowTimer <= 0)
            {
                agent.HasLeftClickInput = false; 
                GetComboSM(agent).ChangeState(ComboStateId.BasicAttack);
                return true;
            }
            return false;
        }

        public override ComboStateId GetId()
        {
            return ComboStateId.FlipAttack;
        }

        public override void Enter(PlayerController playerController)
        {
            base.Enter(playerController);
            AttackData data = playerController.AttackData;
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, GetId()));
            
            var parentState = (AttackingState)playerController.ActionStateMachine.GetState(ActionStateId.Attacking);
            var comboData = data.GetComboStateId(GetId());
            parentState.ResetComboTimer(comboData.attackDelay);
        }

        public override void Update(PlayerController playerController)
        {
            base.Update(playerController);
        }

        public override void Exit(PlayerController agent)
        {
            base.Exit(agent);
        }
    }
}