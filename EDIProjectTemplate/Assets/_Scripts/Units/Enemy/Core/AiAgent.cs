using System.Collections.Generic;
using _Scripts.StateMachine;
using _Scripts.StateMachine.EnemyStatemMachine;
using _Scripts.StateMachine.EnemyStatemMachine.EnemyStates;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Zenject;

namespace _Scripts.Units.Enemy
{
    public class AiAgent : MonoBehaviour

    {
        [Inject] public EnemyManager EnemyManager { get; private set; }
        [Header("This class will be used to register all the different state of every AI")] [Space]
        public EnemyStateId initialState;

        public AiAgentConfig agentConfig;

        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public StateMachine<AiAgent, EnemyStateId> stateMachine;
        [HideInInspector] public NavMeshAgent navMeshAgent;
        private AiVision aiVision;


        private static int nextAgentId = 1;
        private int instanceID;
        private bool playerInSafeZone = false;
        
        [SerializeField] private TextMeshProUGUI statusText;
        public Transform LongRangeTarget;
        [HideInInspector]public Animator animator;

        [HideInInspector]public EnemyAttackTypesData NextAttackTypeData;
        public ChangeLayerChildren ChangeLayerChildren; 
        public bool IsPerformingAttackVisuals { get; set; }
        public bool AttackHasLanded {get; set;}
        public bool IsEnemyUnderAttack = false;

        private void Awake()
        {
            instanceID = nextAgentId;
            nextAgentId++;
        }

        public int GetUniqueID()
        {
            return instanceID;
        }

        void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
            navMeshAgent = GetComponentInParent<NavMeshAgent>();
            aiVision = GetComponent<AiVision>();
            animator = GetComponentInChildren<Animator>();

            stateMachine = new StateMachine<AiAgent, EnemyStateId>(this);
            foreach (var stateId in agentConfig.States)
            {
                stateMachine.RegisterState(StateFactory.Create(stateId));
            }
            
            stateMachine.Initialize(initialState);

        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();
            if (statusText != null)
            {
                statusText.text = $"Current State: {stateMachine.CurrentStateId}";
            }
        }

        public bool IsPlayerDetected(bool angleDoesntMatter = false)
        {
            if (playerInSafeZone)
            {
                return false;
            }

            if (IsEnemyUnderAttack)
            {
                angleDoesntMatter = true;
            }
            return aiVision.IsPlayerDetected(this, angleDoesntMatter);
        }

        public float DistanceToPlayer()
        {
            return (playerTransform.transform.position - transform.position).magnitude;
        }
        public void SetPlayerInSafeZone(bool value)
        {
            playerInSafeZone = value;
        }


    }
    public static class StateFactory
    {
        public static IState<AiAgent, EnemyStateId> Create(EnemyStateId id)
        {
            switch (id)
            {
                case EnemyStateId.Idle: return new EnemyIdleState();
                case EnemyStateId.Wonder: return new EnemyWonderState();
                case EnemyStateId.Charge: return new EnemyChargeState();
                case EnemyStateId.AttackGeneric: return new AttackGeneric();
                case EnemyStateId.CoolDown: return new AttackWindDownState();
                case EnemyStateId.ReadyToAttack: return new ReadyToAttackState();
                case EnemyStateId.Death: return new EnemyDeathState();

                default:
                    Debug.LogError($"State {id} not implemented");
                    return null;
            }
        }
    }
}

