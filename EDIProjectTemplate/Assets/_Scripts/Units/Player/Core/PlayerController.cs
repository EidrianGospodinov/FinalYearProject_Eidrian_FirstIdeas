using System;
using System.Collections.Generic;
using _Scripts.StateMachine;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Player;
using _Scripts.Units.Player.Core;
using _Scripts.Units.Player.View;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CooldownBar cooldownBar;
    [SerializeField] private CooldownBar dashBar;
    [SerializeField] private GameObject cameraFollowOffset;
    [SerializeField] private List<PlayerHealth> healthStates;
    
    
    public AttackData AttackData;
    public HeroData CurrentHeroData;
    [HideInInspector] private AudioSource AudioSource;
    [HideInInspector]public PlayerAnimation playerAnimation;
    [HideInInspector]public bool IsAttacking;
    
    // References to the sub-components
    private PlayerMovement playerMovement;
    public HeroCombinedScript heroCombinedScript { get; private set; }
    [HideInInspector] public ActiveEnemyDetector EnemyDetector;
    [HideInInspector] public HeroSwitcher HeroSwitcher;
   
    private MeshSockets sockets;
    private Transform weaponTransform;
    
    private Vector2 currentMovementInput;

    
    [HideInInspector]public CharacterController CharacterController;
    
    private Vector3 dashVelocity;
    [Inject] private CurrencyManager currencyManager;
    [Inject] public RespawnService RespawnService { get; private set; }
    [Inject] private GameManager gameManager;
    
   // public float DashCooldownEndTime { get; private set; } = 0f;
    //public bool IsDashOnCooldown => Time.time < DashCooldownEndTime;

    public Vector3 CurrentMoveDirection => playerMovement.GetWorldMoveDirection();
    public bool IsWeaponEquipped { get; private set; }
    public bool HasLeftClickInput { get; set; } 
    public bool HasRightClickInput { get; set; }
    public bool HasRightClickHold { get; set; }
    public bool HasDashInput { get; set; }
    public bool HasRunInput { get; set; }
    
    public bool HasSpecialPowerInput { get; set; }
    public bool CanSwitchHero => cooldownBar.IsNotInUse();
    public bool CanUseDash => dashBar.IsNotInUse();
    public Transform GetCameraFollowOffset => cameraFollowOffset.transform;

    // Injected Dependency (PlayerState)
    //[Inject] private PlayerState playerState;

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
        actionStateMachine.RegisterState(new LongRangeAttackState());
        actionStateMachine.RegisterState(new DeathState());
        


        playerMovement = GetComponent<PlayerMovement>();
        heroCombinedScript = GetComponent<HeroCombinedScript>();
        //playerCameraLook = GetComponent<PlayerCameraLook>();
        //playerCombat = GetComponent<PlayerAttack>();
        playerAnimation = GetComponent<PlayerAnimation>();
        CharacterController = GetComponent<CharacterController>();
        HeroSwitcher = GetComponent<HeroSwitcher>();
    }

    private void Start()
    {
        
        //gameManager.SetGameState(GameState.InGame);
        // Set initial state
        actionStateMachine.Initialize(ActionStateId.Ready);
        //playerState = PlayerState.IDLE;
        AudioSource = GetComponent<AudioSource>();
        sockets = GetComponent<MeshSockets>();
        EnemyDetector = GetComponent<ActiveEnemyDetector>();
        
        heroCombinedScript.Init(AttackData.powerUpXpRequired);
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
        
        playerAnimation.SetAnimationIsWalking(isMoving, HasRunInput, IsAttacking);
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
        dashBar.RestartCooldown();
        //DashCooldownEndTime = Time.time + AttackData.playerStats.GetStat(Stat.Dash) ;
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
        playerMovement.HandlePhysics(currentMovementInput, HasRunInput);
    }

    void LateUpdate() 
    { 
       // playerCameraLook.HandleCameraRotation();
       if (weaponTransform == null) return;
       var weaponManager = weaponTransform.GetComponent<WeaponManager>();
       float normalizedXp = Mathf.Clamp01(heroCombinedScript.currentPowerUpXp / AttackData.powerUpXpRequired);
       float intensity = normalizedXp * 7;
       weaponManager.UpdateSwordIntensity(intensity);
    }
    
    public void SetMovementInput(Vector2 input)
    {
        currentMovementInput = input;
    }

    public void Jump()
    {
        playerMovement.Jump();
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
    private EventBinding<OnSwitchHeroEvent> playerEventBinding;

    private void OnEnable()
    {
        playerEventBinding = EventBus<OnSwitchHeroEvent>.Register(HandleHeroSwitchEvent);
    }

    private void HandleHeroSwitchEvent(OnSwitchHeroEvent obj)
    {
        cooldownBar.RestartCooldown();
        CurrentHeroData = obj.HeroData;
    }
    public void ResetState()
    {
        foreach (var health in healthStates)
        {
            health.ResetHealth();
        }
        
    }

    
}