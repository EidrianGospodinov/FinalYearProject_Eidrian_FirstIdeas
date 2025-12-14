using _Scripts.StateMachine.PlayerActionStateMachine;
using Unity.VisualScripting;

namespace _Scripts.Units.Player
{
    using UnityEngine;

    public class PlayerAnimation : MonoBehaviour
    {
        private Animator animator;
        private MeshSockets sockets;

        // Animation Constants
        public const string IDLE = "Idle";
        public const string WALK = "Walk";
        public const string COOLDOWN = "Cooldown";
        public const string ATTACK1 = "BasicAttack";
        public const string ATTACK2 = "SecondaryAttack"; 
        private const string WEAPON_LAYER_NAME = "Weapon Layer";
        private const string BaseWeapon_LAYER_NAME = "Base Layer Sword";

        private string currentAnimationState;
        
        private EventBinding<OnAttack> OnAttack;

        void Awake()
        {
            animator = GetComponent<Animator>();
            sockets = GetComponent<MeshSockets>();
        }
        void OnEnable()
        {
            OnAttack = EventBus<OnAttack>.Register(OnAttackEvent);
        }

        private void OnAttackEvent(OnAttack evt)
        {
            var comboId = evt.ComboStateId;
            string animState = "";
            switch (comboId)
            {
                case ComboStateId.WindDown:
                    animState = COOLDOWN;
                    break;
                case ComboStateId.BasicAttack:
                    animState = ATTACK1;
                    break;
                case ComboStateId.SecondaryBasicAttack:
                    animState = ATTACK2;
                    break;
            }

            if (animState != "")
            {
                ChangeAnimationState(animState, WEAPON_LAYER_NAME);
            }
        }

        void OnDisable()
        {
            EventBus<OnAttack>.Unregister(OnAttack);
        }

        // Called by PlayerController in Update()
        public void SetAnimationIsWalking(bool isMoving, bool isAttacking)
        {
            // if the player is attacking, don't set any animations
            if (isAttacking)
            {
                return;
            }

            // Movement state
            animator.SetBool("isWalking", isMoving);
                //ChangeAnimationState(WALK);
            
        }

        public void ChangeAnimationState(string newState, string layerName = "Base Layer") 
        {
            if (currentAnimationState == newState)
            {
                return;
            }

            currentAnimationState = newState;
            if (currentAnimationState != COOLDOWN)
            {
                animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
            }
        }

        public void ActivateWeapon(Transform WeaponTransform,bool shouldEquip)
        {
            WeaponTransform.localPosition = Vector3.zero;
            
            //change the synced based layer based on weapon
            var baseWeaponLayerIndex = animator.GetLayerIndex(BaseWeapon_LAYER_NAME);
            if (shouldEquip)
            {
                animator.SetLayerWeight(baseWeaponLayerIndex, 1);
                sockets.Attach(WeaponTransform, MeshSockets.SocketId.Spine);
                
            }
            else
            {
                animator.SetLayerWeight(baseWeaponLayerIndex, 0);
                sockets.Attach(WeaponTransform, MeshSockets.SocketId.RightHand);
            }
            animator.SetBool("isEquip", shouldEquip);
        }

        
    }
}