using _Scripts.StateMachine.PlayerActionStateMachine;
using Unity.VisualScripting;

namespace _Scripts.Units.Player
{
    using UnityEngine;

    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;

        // Animation Constants
        public const string IDLE = "Idle";
        public const string WALK = "Walk";
        public const string ATTACK1 = "BasicAttack";
        public const string ATTACK2 = "SecondaryAttack"; 

        private string currentAnimationState;
        
        private EventBinding<OnAttack> OnAttack;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void OnEnable()
        {
            OnAttack = EventBus<OnAttack>.Register(OnAttackEvent);
        }

        private void OnAttackEvent(OnAttack evt)
        {
            var comboId = evt.ComboStateId;
            string animState = IDLE;
            switch (comboId)
            {
                case ComboStateId.BasicAttack:
                    animState = ATTACK1;
                    break;
                case ComboStateId.SecondaryBasicAttack:
                    animState = ATTACK2;
                    break;
            }
            ChangeAnimationState(animState);
        }

        void OnDisable()
        {
            EventBus<OnAttack>.Unregister(OnAttack);
        }

        // Called by PlayerController in Update()
        public void SetAnimations(bool isMoving, bool isAttacking)
        {
            // if the player is attacking, don't set any animations
            if (isAttacking)
            {
                return;
            }

            // Movement state
            if (isMoving)
            {
                ChangeAnimationState(WALK);
            }
            else
            {
                ChangeAnimationState(IDLE);
            }
        }

        public void ChangeAnimationState(string newState) 
        {
            if (currentAnimationState == newState)
            {
                return;
            }

            currentAnimationState = newState;
            animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
        }
    }
}