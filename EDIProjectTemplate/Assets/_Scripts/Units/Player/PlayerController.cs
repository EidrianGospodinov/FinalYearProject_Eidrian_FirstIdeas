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
    public HeroData CurrentHeroData;
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
        actionStateMachine.RegisterState(new DashingState());
        


        playerMovement = GetComponent<PlayerMovement>();
        //playerCameraLook = GetComponent<PlayerCameraLook>();
        //playerCombat = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
        CharacterController = GetComponent<CharacterController>();
        

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


        // Determine animation state based on component data
        bool isMoving = playerMovement.IsMoving;
        playerAnimation.SetBoolParam("isJumping", playerMovement.IsJumping);
        
        playerAnimation.SetAnimationIsWalking(isMoving, IsAttacking);
    }

    public void FirstTimeEquipWeapon()
    {
        if (weaponTransform == null)
        {
            var weaponInstance = Instantiate(AttackData.WeaponPrefab);
            weaponInstance.GetComponent<OnHit>().Initialize(AttackData);
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
    private EventBinding<OnSwitchHeroEvent> playerEventBinding;

    private void OnEnable()
    {
        playerEventBinding = EventBus<OnSwitchHeroEvent>.Register(HandleHeroSwitchEvent);
    }

    private void HandleHeroSwitchEvent(OnSwitchHeroEvent obj)
    {
        CurrentHeroData = obj.HeroData;
    }

    
}