using System;
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
        private const string IDLE = "Idle";
        private const string WALK = "Walk";
        private const string COOLDOWN = "Cooldown";
        private const string ATTACK1 = "BasicAttack";
        private const string ATTACK2 = "SecondaryAttack"; 
        private const string ATTACK3 = "SpecialAttackHighSpin";
        private const string ATTACKSPECIAL2 = "FlipAttack";
        private const string LongRangeAttackHold = "LongRangeAttackPoseStart";
        private const string ThaliaThunderSpecialAbility = "SpecialAbility_Thunder";

        private static readonly string[] Death_Sword = new[] { "Death_back", "Death_front" };
        private static readonly string[] Death_NoSword = new[] { "Death_back", "Death_front" };
       
        
        
            
        
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
                case ComboStateId.None:
                    if (evt.AttackType == AttackType.LongRange)
                    {
                        animState = LongRangeAttackHold;
                    }
                    else if (evt.AttackType == AttackType.Special)
                    {
                        if (evt.HeroType == HeroType.Oreon)
                        {
                            
                        }
                        else if(evt.HeroType == HeroType.Thalia)
                        {
                            animState = ThaliaThunderSpecialAbility;
                        }
                    }
                    break;
                case ComboStateId.WindDown:
                    animState = COOLDOWN;
                    break;
                case ComboStateId.BasicAttack:
                    animState = ATTACK1;
                    break;
                case ComboStateId.SecondaryBasicAttack:
                    animState = ATTACK2;
                    break;
                case ComboStateId.SpecialAttack:
                    animState = ATTACK3;
                    break;
                case ComboStateId.FlipAttack:
                    animState = ATTACKSPECIAL2;
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

        public void Update()
        {
        }

        // Called by PlayerController in Update()
        public void SetAnimationIsWalking(bool isMoving, bool hasRunInput, bool isAttacking)
        {
            // if the player is attacking, don't set any animations
            if (isAttacking)
            {
                return;
            }

            float locomotionValue = 0;
            if (isMoving)
            {
                locomotionValue = hasRunInput ? 2f : 1f;
            }
            // Movement state
            float currentVal = animator.GetFloat("Locomotion");
            float smoothedVal = Mathf.Lerp(currentVal, locomotionValue, Time.deltaTime * 10f);
            
            animator.SetFloat("Locomotion", smoothedVal);
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

        public void SetBoolParam(string name, bool isDoing)
        {
            animator.SetBool(name, isDoing);
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


        public void PlayDeathAnim(bool IsWeaponEquipped)
        {
            var randomNum = Random.Range(0, 2);
            string animName = "";
            if (IsWeaponEquipped)
            {
                animName = Death_Sword[randomNum];
            }
            else
            {
                animName = Death_NoSword[randomNum];
            }

            if (animName != "")
            {
                animator.CrossFadeInFixedTime(animName, 0.2f);
            }
        }
    }
}