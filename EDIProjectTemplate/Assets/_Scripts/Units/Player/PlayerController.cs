using System;
using _Scripts.StateMachine;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Player;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public AttackData AttackData;
    public AudioSource AudioSource;
    public Camera Camera;
    public PlayerAnimation playerAnimation;
    public bool IsAttacking;
    public Animator Animator;
    
    // References to the sub-components
    private PlayerMovement playerMovement;
    private PlayerCameraLook playerCameraLook;
    private PlayerAttack playerCombat;
    // Input System
    private PlayerInput playerInput;
    private PlayerInput.MainActions input;
    public bool HasLeftClickInput { get; set; } 
    public bool HasRightClickInput { get; set; } 

    // Injected Dependency (PlayerState)
    [Inject] private PlayerState playerState;

    private StateMachine<PlayerController, ActionStateId> actionStateMachine;

    public StateMachine<PlayerController, ActionStateId> ActionStateMachine
    {
        get
        {
            if (actionStateMachine != null)
            {
                return actionStateMachine;
            }
            throw new InvalidOperationException($"State machine is null");
        }
    }

    void Awake()
    {
        actionStateMachine = new StateMachine<PlayerController, ActionStateId>(this);
        actionStateMachine.RegisterState(new AttackingState());
        actionStateMachine.RegisterState(new ReadyState());
        


        Camera = Camera.main; 
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraLook = GetComponent<PlayerCameraLook>();
        playerCombat = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
        
        playerInput = new PlayerInput();
        input = playerInput.Main;

        AssignInputs();
    }

    private void Start()
    {
        // Set initial state
        actionStateMachine.Initialize(ActionStateId.Ready);
        playerState = PlayerState.IDLE;
        AudioSource = GetComponent<AudioSource>();
        Animator = GetComponent<Animator>();
        
        
    }

    void Update()
    {
        actionStateMachine.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventBus<TestEvent>.Trigger(new TestEvent());
        }

        // Pass input values to the relevant components
        playerMovement.SetMovementInput(input.Movement.ReadValue<Vector2>());
        playerCameraLook.SetLookInput(input.Look.ReadValue<Vector2>());

        // Determine animation state based on component data
        bool isMoving = playerMovement.IsMoving;
        
        playerAnimation.SetAnimations(isMoving, IsAttacking);
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
        input.Attack.started += ctx => HasLeftClickInput = true;
        input.SecondaryAttack.started += ctx => HasRightClickInput = true;
    }

    void OnEnable() 
    { input.Enable(); }

    void OnDisable()
    { input.Disable(); }
}