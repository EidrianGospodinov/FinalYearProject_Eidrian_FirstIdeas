using _Scripts.StateMachine;
using _Scripts.StateMachine.EnemyStatemMachine;
using _Scripts.StateMachine.EnemyStatemMachine.EnemyStates;
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



            //state machine set up- register all stated this agent will use
            stateMachine = new StateMachine<AiAgent, EnemyStateId>(this);
            stateMachine.RegisterState(new EnemyWonderState());
            stateMachine.RegisterState(new EnemyIdleState());
            stateMachine.RegisterState(new ReadyToAttackState());
            stateMachine.RegisterState(new EnemyChargeState());
            stateMachine.Initialize(initialState);

        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();
        }

        public bool IsPlayerDetected()
        {
            return aiVision.IsPlayerDetected(this);
        }

        public float DistanceToPlayer()
        {
            return (playerTransform.transform.position - transform.position).magnitude;
        }


    }
}

