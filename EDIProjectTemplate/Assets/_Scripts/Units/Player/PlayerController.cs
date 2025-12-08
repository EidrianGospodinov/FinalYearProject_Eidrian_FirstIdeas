using _Scripts.Units.Player;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    // References to the sub-components
    private PlayerMovement playerMovement;
    private PlayerCameraLook playerCameraLook;
    private PlayerAttack playerCombat;
    private PlayerAnimation playerAnimation;

    // Input System
    private PlayerInput playerInput;
    private PlayerInput.MainActions input;

    // Injected Dependency (PlayerState)
    [Inject] private PlayerState playerState;

    void Awake()
    {
        // 1. Get references to the components on this GameObject
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraLook = GetComponent<PlayerCameraLook>();
        playerCombat = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
        
        // 2. Setup Input System
        playerInput = new PlayerInput();
        input = playerInput.Main;

        // 3. Setup Input Bindings (The Coordinator's Job)
        AssignInputs();
    }

    private void Start()
    {
        // Set initial state
        playerState = PlayerState.IDLE;
    }

    void Update()
    {
        // Example: EventBus testing (can be moved to a dedicated Debug/Event component later)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { EventBus<TestEvent>.Trigger(new TestEvent()); }
        
        // Pass input values to the relevant components
        playerMovement.SetMovementInput(input.Movement.ReadValue<Vector2>());
        playerCameraLook.SetLookInput(input.Look.ReadValue<Vector2>());

        // Determine animation state based on component data
        bool isMoving = playerMovement.IsMoving;
        bool isAttacking = playerCombat.IsAttacking;
        
        playerAnimation.SetAnimations(isMoving, isAttacking);
    }

    void FixedUpdate() 
    { 
        playerMovement.HandlePhysics();
    }

    void LateUpdate() 
    { 
        playerCameraLook.HandleCameraRotation();
    }
    
    // --- Input Management ---

    void AssignInputs()
    {
        // Call the specific component's method when input is performed
        input.Jump.performed += ctx => playerMovement.Jump();
        input.Attack.started += ctx => playerCombat.Attack();
    }

    void OnEnable() 
    { input.Enable(); }

    void OnDisable()
    { input.Disable(); }
}