using System;
using _Scripts.StateMachine;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public AttackData AttackData;
    [HideInInspector] public AudioSource AudioSource;
    [HideInInspector]public Camera Camera;
    [HideInInspector]public PlayerAnimation playerAnimation;
    [HideInInspector]public bool IsAttacking;
    [HideInInspector]public Animator Animator;
    
    // References to the sub-components
    private PlayerMovement playerMovement;
    //private PlayerCameraLook playerCameraLook;
    //private PlayerAttack playerCombat;
    
    
    //dodge 
    [HideInInspector]public CharacterController CharacterController;
    [Header("Dodge Settings")]
    [SerializeField] private float dodgeCooldownDuration = 1f;
    [SerializeField] private float dodgeSpeed = 7f; 
    private Vector3 dodgeVelocity;
    
    public float DodgeCooldownEndTime { get; private set; } = 0f;
    public bool IsDodgeOnCooldown => Time.time < DodgeCooldownEndTime;

    public Vector3 CurrentMoveDirection => playerMovement.GetWorldMoveDirection();
    // Input System
    private PlayerInput playerInput;
    private PlayerInput.MainActions input;
    public bool HasLeftClickInput { get; set; } 
    public bool HasRightClickInput { get; set; }
    public bool HasDashInput { get; set; }

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
        actionStateMachine.RegisterState(new DodgingState());
        


        Camera = Camera.main; 
        playerMovement = GetComponent<PlayerMovement>();
        //playerCameraLook = GetComponent<PlayerCameraLook>();
        //playerCombat = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
        CharacterController = GetComponent<CharacterController>();
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
            playerAnimation.ActivateWeapon(true);
            EventBus<TestEvent>.Trigger(new TestEvent());
        }

        // Pass input values to the relevant components
        playerMovement.SetMovementInput(input.Movement.ReadValue<Vector2>());
        //playerCameraLook.SetLookInput(input.Look.ReadValue<Vector2>());

        // Determine animation state based on component data
        bool isMoving = playerMovement.IsMoving;
        
        playerAnimation.SetAnimations(isMoving, IsAttacking);
    }

    public void PlayAnimation(string animationName)
    {
        playerAnimation.ChangeAnimationState(animationName);
    }
    public void SetDodgeCooldown()
    {
        DodgeCooldownEndTime = Time.time + dodgeCooldownDuration;
    }
    public void PerformDashMovement(float duration)
    {
        Vector3 dodgeDirection;
        
        if (CurrentMoveDirection.sqrMagnitude > 0.01f)
        {
            dodgeDirection = CurrentMoveDirection.normalized;
        }
        else
        {
            dodgeDirection = transform.forward;
        }

        
        dodgeVelocity = dodgeDirection * dodgeSpeed;
        
        
        
    }
    public Vector3 DodgeVelocity => dodgeVelocity;

    void FixedUpdate() 
    { 
        playerMovement.HandlePhysics();
    }

    void LateUpdate() 
    { 
       // playerCameraLook.HandleCameraRotation();
    }
    
    // --- Input Management ---

    void AssignInputs()
    {
        // Call the specific component's method when input is performed
        input.Jump.performed += ctx => playerMovement.Jump();
        input.Attack.started += ctx => HasLeftClickInput = true;
        input.SecondaryAttack.started += ctx => HasRightClickInput = true;
        input.Dash.started += ctx =>
        {
            if (IsAttacking || IsDodgeOnCooldown) 
                //prevent dodge happening right after an attack even with the button pressed during that attack
            {
                return;
            }

            HasDashInput = true;
        };
    }

    void OnEnable() 
    { input.Enable(); }

    void OnDisable()
    { input.Disable(); }
}