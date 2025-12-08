namespace _Scripts.Units.Player
{
    using UnityEngine;

    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;

        // Animation Constants
        public const string IDLE = "Idle";
        public const string WALK = "Walk";
        public const string ATTACK1 = "Attack 1";
        public const string ATTACK2 = "Attack 2"; 

        private string currentAnimationState;
        
        private EventBinding<OnAttack> OnAttack;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void OnEnable()
        {
            OnAttack = new EventBinding<OnAttack>(OnAttackEvent);
            EventBus<OnAttack>.Register(OnAttack);
        }

        private void OnAttackEvent(OnAttack evt)
        {
            if (evt.SequenceID == -1) return;
            string animState = (evt.SequenceID == 1) ? ATTACK2 : ATTACK1;   
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