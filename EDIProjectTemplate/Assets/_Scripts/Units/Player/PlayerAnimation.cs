namespace _Scripts.Units.Player
{
    using UnityEngine;

    public class PlayerAnimation : MonoBehaviour
    {
       [SerializeField] private Animator animator;

        // Animation Constants
        public const string IDLE = "Idle";
        public const string WALK = "Walk";
        public const string ATTACK1 = "Attack 1";
        public const string ATTACK2 = "Attack 2"; // Use ATTACK1 or ATTACK2 based on PlayerCombat state

        private string currentAnimationState;
        
        private EventBinding<OnAttack> OnAttack;

        void Awake()
        {
        }
        void OnEnable()
        {
            // Subscribe to the event when the component is enabled
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
            // Unsubscribe when the component is disabled
            EventBus<OnAttack>.Unregister(OnAttack);
        }

        // Called by PlayerController in Update()
        public void SetAnimations(bool isMoving, bool isAttacking)
        {
            // Attacking state overrides all other animations
            if (isAttacking)
            {
                // Note: The specific attack animation (ATTACK1 or ATTACK2) 
                // is usually decided *within* the Attack() function, but 
                // for simplicity here, we'll just check if attacking is true.
                // If you need the specific attack state, PlayerCombat must expose it.
                // ChangeAnimationState(PlayerCombat.CurrentAttackAnimation); 
                return;
            }

            // Movement state
            if(isMoving)
            { ChangeAnimationState(WALK); }
            else
            { ChangeAnimationState(IDLE); }
        }

        public void ChangeAnimationState(string newState) 
        {
            if (currentAnimationState == newState) return;

            currentAnimationState = newState;
            animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
        }
    }
}