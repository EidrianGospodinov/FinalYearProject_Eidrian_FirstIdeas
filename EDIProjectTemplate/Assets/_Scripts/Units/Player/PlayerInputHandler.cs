using System;
using UnityEngine;

namespace _Scripts.Units.Player
{
    public class PlayerInputHandler: MonoBehaviour
    {
        
        private PlayerMovement playerMovement; 
        private PlayerController playerController;
        private PlayerInput playerInput;
        private PlayerInput.MainActions input;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            playerMovement = GetComponent<PlayerMovement>();
            playerInput = new PlayerInput();
            input = playerInput.Main;
            AssignInputs();
        }

        private void Update()
        {
            playerMovement.SetMovementInput(input.Movement.ReadValue<Vector2>());
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
            input.Jump.performed += ctx => playerMovement.Jump();
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
            input.Dash.started += ctx =>
            {
                if (playerController.IsAttacking || playerController.IsDodgeOnCooldown) 
                    //prevent dodge happening right after an attack even with the button pressed during that attack
                {
                    return;
                }

                playerController.HasDashInput = true;
            };
        }
    }
}