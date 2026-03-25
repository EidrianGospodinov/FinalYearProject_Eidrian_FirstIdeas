using UnityEngine;

namespace _Scripts.StateMachine.PlayerActionStateMachine.AttackComboStates
{
    public abstract class BaseCombatAttackState : IState<PlayerController,ComboStateId>
    {
        protected AttackingState GetParentState(PlayerController agent)
        {
            return (AttackingState)agent.ActionStateMachine.GetState(ActionStateId.Attacking);
        }
    
        protected StateMachine<PlayerController, ComboStateId> GetComboSM(PlayerController agent)
        {
            return GetParentState(agent).GetComboStateMachine;
        }
        protected abstract bool TryTransitionToNextState(PlayerController agent);
        public abstract ComboStateId GetId();

        public virtual void Enter(PlayerController playerController)
        {
            Debug.Log("set is attacking to true");
            playerController.IsAttacking = true;
            playerController.PlayAudioSource(playerController.AttackData.swordSwing);
            
        }

        public virtual void Update(PlayerController playerController)
        {
            if (TryTransitionToNextState(playerController))
            {
                return;
            }
            
            if (GetParentState(playerController).ComboWindowTimer <= 0)
            {
                GetComboSM(playerController).ChangeState(ComboStateId.WindDown);
            }
        }

        public virtual void Exit(PlayerController agent)
        {
            EventBus<OnAttack>.Trigger(new OnAttack(AttackType.NONE, ComboStateId.WindDown));
            Debug.Log("set is attacking to false");
            agent.IsAttacking = true;
        }
    }
}