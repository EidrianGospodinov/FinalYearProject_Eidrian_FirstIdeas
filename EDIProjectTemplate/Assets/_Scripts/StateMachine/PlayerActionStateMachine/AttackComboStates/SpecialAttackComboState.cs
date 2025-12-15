using _Scripts.Units.Player;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public class SpecialAttackComboState : BaseCombatAttackState
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
            return ComboStateId.SpecialAttack;        }

        public override void Enter(PlayerController agent)
        {
            base.Enter(agent);
            AttackData data = agent.AttackData;
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, GetId()));
            
            var parentState = (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
            var comboData = data.GetComboStateId(GetId());
            parentState.ResetComboTimer(comboData.attackDelay);
        }

        public override void Update(PlayerController agent)
        {
            base.Update(agent);
        }

        public override void Exit(PlayerController agent)
        {
            base.Exit(agent);
        }
    }
}