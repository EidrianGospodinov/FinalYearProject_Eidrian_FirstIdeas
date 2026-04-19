using System;
using System.Collections.Generic;
using _Scripts.StateMachine;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Player;
using _Scripts.Units.Player.Core;
using _Scripts.Units.Player.View;
using _Scripts.Units.Sound.Footstep;
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
    [SerializeField] private Stats playerStats;
    
    
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
    private WeaponManager weaponManager;
    
    private Vector2 currentMovementInput;

    
    [HideInInspector]public CharacterController CharacterController;
    
    private Vector3 dashVelocity;
    [Inject] private CurrencyManager currencyManager;
    [Inject] public RespawnService RespawnService { get; private set; }
    [Inject] private GameManager gameManager;
    [Inject] private GameObjectSpawner_DI gameObjectSpawnerDi;
    [Inject] public PlayerServices PlayerServices { get; set; }
    
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
        playerStats.upgradeApplied += StatsUpgradeApplied;
        //gameManager.SetGameState(GameState.InGame);
        // Set initial state
        actionStateMachine.Initialize(ActionStateId.Ready);
        //playerState = PlayerState.IDLE;
        AudioSource = GetComponent<AudioSource>();
        sockets = GetComponent<MeshSockets>();
        EnemyDetector = GetComponent<ActiveEnemyDetector>();
        heroCombinedScript.Init(playerStats);
    }

    private void StatsUpgradeApplied(Stats arg1, StatsUpgrade arg2)
    {
        //todo: use the params from the event instead of getting the value each time a upgrade happens 
        heroCombinedScript.UpdatePowerUpXpRequired();
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
        if (weaponManager == null)
        {
            //i need to use diContainer for the prefab instantiate, but i should not do it in playerController
            var weaponInstance= gameObjectSpawnerDi.Spawn(AttackData.WeaponPrefab);
            weaponInstance.GetComponent<OnPlayerHittingEnemy>().Initialize(AttackData);
            weaponManager = weaponInstance.GetComponent<WeaponManager>();
            heroCombinedScript.InitSwordFound(weaponManager);
            EquipWeapon();
        }
    }
    public void EquipWeapon()
    {
        if (weaponManager != null)
        {
            IsWeaponEquipped = !IsWeaponEquipped;
            playerAnimation.ActivateWeapon(weaponManager.transform, IsWeaponEquipped!);
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
       
       //todo: keeping this here until i have time to fix it
       heroCombinedScript.UpdateSwordIntensity();
       /*if (weaponManager == null) return;
       float normalizedXp = Mathf.Clamp01(heroCombinedScript.currentPowerUpXp / AttackData.powerUpXpRequired);
       float intensity = normalizedXp * 7;
       weaponManager.UpdateSwordIntensity(intensity);*/
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
            if (weaponManager == null)
            {
                return;
            }

            weaponManager.transform.localPosition = Vector3.zero;
            if (IsWeaponEquipped)
            {
                sockets.Attach(weaponManager.transform, MeshSockets.SocketId.RightHand);
            }
            else
            {
                sockets.Attach(weaponManager.transform, MeshSockets.SocketId.Spine);
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
        heroCombinedScript.UpdateSwordIntensity();
    }
    public void ResetState()
    {
        ActionStateMachine.ChangeState(ActionStateId.Ready);
        foreach (var health in healthStates)
        {
            health.ResetHealth();
        }
        
    }


    public void SetDeathState()
    {
        gameManager.SetGameState(GameState.Death);
    }

    public void SwapCollection(FootstepCollection collection)
    {
        playerMovement.SwapCollection(collection);
    }
}