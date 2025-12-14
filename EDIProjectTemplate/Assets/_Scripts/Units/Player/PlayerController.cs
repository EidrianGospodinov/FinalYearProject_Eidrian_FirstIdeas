using System;
using _Scripts.StateMachine;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public AttackData AttackData;
    [HideInInspector] private AudioSource AudioSource;
    [HideInInspector]public PlayerAnimation playerAnimation;
    [HideInInspector]public bool IsAttacking;
    
    // References to the sub-components
    private PlayerMovement playerMovement;
   
    private MeshSockets sockets;
    private Transform weaponTransform;
    
    [HideInInspector]public CharacterController CharacterController;
    
    private Vector3 dashVelocity;
    
    public float DodgeCooldownEndTime { get; private set; } = 0f;
    public bool IsDodgeOnCooldown => Time.time < DodgeCooldownEndTime;

    public Vector3 CurrentMoveDirection => playerMovement.GetWorldMoveDirection();
    public bool IsWeaponEquipped { get; private set; }
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
        sockets = GetComponent<MeshSockets>();
        
        
    }

    void Update()
    {
        actionStateMachine.Update();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EquipWeapon();

            EventBus<TestEvent>.Trigger(new TestEvent());
        }

        // Pass input values to the relevant components
        playerMovement.SetMovementInput(input.Movement.ReadValue<Vector2>());

        // Determine animation state based on component data
        bool isMoving = playerMovement.IsMoving;
        
        playerAnimation.SetAnimationIsWalking(isMoving, IsAttacking);
    }

    public void FirstTimeEquipWeapon()
    {
        if (weaponTransform == null)
        {
            var weaponInstance = Instantiate(AttackData.WeaponPrefab);
            weaponTransform = weaponInstance.transform;
            EquipWeapon();
        }
    }
    public void EquipWeapon()
    {
        if (weaponTransform != null)
        {
            IsWeaponEquipped = !IsWeaponEquipped;
            playerAnimation.ActivateWeapon(weaponTransform, IsWeaponEquipped!);
        }
    }

    public void PlayAnimation(string animationName)
    {
        playerAnimation.ChangeAnimationState(animationName);
    }
    public void SetDodgeCooldown()
    {
        DodgeCooldownEndTime = Time.time + AttackData.dashCooldownDuration;
    }

    public void PlayAudioSource(AudioClip audioClip)
    {
        AudioSource.pitch = Random.Range(0.9f, 1.1f);
        AudioSource.PlayOneShot(audioClip);
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

        
        dashVelocity = dodgeDirection * AttackData.dashSpeed;
        
        
        
    }
    public Vector3 DashVelocity => dashVelocity;

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
        input.Attack.started += ctx =>
        {
            if (IsWeaponEquipped)
            {
                HasLeftClickInput = true;
            }
        };
        input.SecondaryAttack.started += ctx => 
        {
            if (IsWeaponEquipped)
            {
                HasRightClickInput = true;
            }
        };
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
    public void OnFirstHalfOfEquipEventFinish(string eventName)
    {
        if (eventName == "equipWeapon")
        {
            if (weaponTransform == null)
            {
                return;
            }

            weaponTransform.transform.localPosition = Vector3.zero;
            if (IsWeaponEquipped)
            {
                sockets.Attach(weaponTransform.transform, MeshSockets.SocketId.RightHand);
            }
            else
            {
                sockets.Attach(weaponTransform.transform, MeshSockets.SocketId.Spine);
            }
        }
    }
    void OnEnable() 
    { input.Enable(); }

    void OnDisable()
    { input.Disable(); }
}