using _Scripts.StateMachine;
using _Scripts.StateMachine.EnemyStatemMachine;
using _Scripts.StateMachine.EnemyStatemMachine.EnemyStates;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Scripts.Units.Enemy
{
    public class AiAgent : MonoBehaviour

    {
        [Header("This class will be used to register all the different state of every AI")] [Space]
        public EnemyStateId initialState;

        public AiAgentConfig agentConfig;

        [HideInInspector] public Transform playerTransform;
        [HideInInspector] public StateMachine<AiAgent, EnemyStateId> stateMachine;
        [HideInInspector] public NavMeshAgent navMeshAgent;
        private AiVision aiVision;

        [Header("Shooting Components")] public GameObject projectilePrefab;
        public Transform bulletOffset;

        private static int nextAgentId = 1;
        private int instanceID;
        
        [SerializeField] private TextMeshProUGUI statusText;
        public Animator animator;

        public EnemyAttackTypesData NextAttackTypeData;
        public bool IsPerformingAttackVisuals { get; set; }
        public bool AttackHasLanded {get; set;}

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



            //state machine set up- register all stated this agent will use
            stateMachine = new StateMachine<AiAgent, EnemyStateId>(this);
            stateMachine.RegisterState(new EnemyWonderState());
            stateMachine.RegisterState(new EnemyIdleState());
            stateMachine.RegisterState(new ReadyToAttackState());
            stateMachine.RegisterState(new EnemyChargeState());
            stateMachine.RegisterState(new AttackWindDownState());
            stateMachine.RegisterState(new AttackGeneric());
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

        public bool IsPlayerDetected(bool doesAngleMatter = false)
        {
            return aiVision.IsPlayerDetected(this, doesAngleMatter);
        }

        public float DistanceToPlayer()
        {
            return (playerTransform.transform.position - transform.position).magnitude;
        }


    }
}

