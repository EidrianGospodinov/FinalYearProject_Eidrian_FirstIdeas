using System.Collections.Generic;
using _Scripts.StateMachine.EnemyStatemMachine;
using UnityEngine;

namespace _Scripts.Units.Enemy
{
    [CreateAssetMenu]
    public class AiAgentConfig : ScriptableObject
    {
        [Header("Stats")]
        public float Height = 2.0f;
     
        [Header("States")]
        public List<EnemyStateId> States;
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
        [Range(0, 1)] public float critChance;
        public List<EnemyAttackTypesData> EnemyAttackTypes;
        
        [Header("Death State")]
        public string DeathStateAnimationName = "Death";
        public int DeathAnimDuration = 3;

        public DynamicTextData DynamicTextData;
        public DynamicTextData CritData;

    }
}