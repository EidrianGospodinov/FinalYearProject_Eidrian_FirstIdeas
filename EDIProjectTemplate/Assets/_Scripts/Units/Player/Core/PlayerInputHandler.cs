using System;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

namespace _Scripts.Units.Player
{
    public class PlayerInputHandler: MonoBehaviour
    {
        [Inject] private GameManager gameManager;
        private PlayerController playerController;
        private PlayerInput playerInput;
        private PlayerInput.MainActions input;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            playerInput = new PlayerInput();
            input = playerInput.Main;
            AssignInputs();
        }

        private void Update()
        {
            if (gameManager.GetCurrentGameState != GameState.InGame)
            {
                return;
            }

            playerController.HasRunInput = input.Run.IsPressed();
            playerController.SetMovementInput(input.Movement.ReadValue<Vector2>());
        }

        void OnEnable()
        {
            input.Enable();
        }

        void OnDisable()
        {
            input.Disable();
        }

        void AssignInputs()
        {
            // Call the specific component's method when input is performed
            input.Jump.performed += ctx => playerController.Jump();
            input.Attack.started += ctx =>
            {
                if (playerController.IsWeaponEquipped)
                {
                    playerController.HasLeftClickInput = true;
                }
            };
            input.SecondaryAttack.started += ctx => 
            {
                if (playerController.IsWeaponEquipped)
                {
                    playerController.HasRightClickInput = true;
                }
            };
            input.SecondaryAttack.performed += ctx =>
            {
                if (ctx.interaction is UnityEngine.InputSystem.Interactions.HoldInteraction)
                {
                    playerController.HasRightClickHold = true;
                    playerController.HasRightClickInput = false;
                }
            };
            input.SecondaryAttack.canceled += ctx =>
            {
                playerController.HasRightClickHold = false;
                playerController.playerAnimation.SetBoolParam("isHoldingRightMouseButton", false );
                EventBus<OnAttack>.Trigger(new OnAttack(AttackType.NONE));
            };
            input.Dash.started += ctx =>
            {
                if (playerController.IsAttacking || !playerController.CanUseDash) 
                    //prevent dodge happening right after an attack even with the button pressed during that attack
                {
                    return;
                }

                playerController.HasDashInput = true;
            };
            input.PlayerSwitch.started += ctx =>
            {
                if (playerController.CanSwitchHero)
                {
                    playerController.HeroSwitcher.RequestHeroSwitch();
                }
            };
            input.SpecialPower.started += ctx =>
            {
                if (playerController.heroCombinedScript.CanPowerUp)
                {
                    playerController.HasSpecialPowerInput = true;
                }
            };
        }
    }
}