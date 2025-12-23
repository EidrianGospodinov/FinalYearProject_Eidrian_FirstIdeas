using UnityEngine;

namespace _Scripts.StateMachine.EnemyStatemMachine
{
    [CreateAssetMenu]
    public class AiAgentConfig : ScriptableObject
    {
        [Header("Wander State")]
        public float wanderRadius = 15f;
        public float minWanderRadius = 5f;
        public float destinationRefreshTime = 3f;
        
        [Header("Ai Vision")]
        public float angleVision = 90;
        public float maxSightDistance = 5;
        public LayerMask obstacleLayer;

        [Header("Idle State")] 
        public bool stayForeverIdle = false;
        public float minIdleTime = 1;
        public float maxIdleTime = 5;

        [Header("Chase State")]
        public float timeToLosePlayer = 5;
        public float attackRange = 5;
        
        [Header("Attack State")]
        public float fireRate = 1;
        public float turningSpeed = 1;
        public float bulletDamage = 25;
        public float bulletSpeed = 5;
        [Range(0, 1)] public float critChance;
    }
}