using _Scripts.StateMachine.EnemyStatemMachine;
using TMPro;
using UnityEngine;

namespace _Scripts.Units.Enemy
{
    public class AiVision : MonoBehaviour
    {

        public GameObject Player;



        public bool IsPlayerDetected(AiAgent agent) => IsPlayerDetected(agent.agentConfig);


        public bool IsPlayerDetected(AiAgentConfig config)
        {
            Vector3 directionToPlayer = Player.transform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer > config.maxSightDistance)
            {
                return false;
            }

            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer > config.angleVision / 2f)
            {
                return false;
            }

            Vector3 eyePosition = transform.position + Vector3.up * 1f;
            if (Physics.Raycast(eyePosition, directionToPlayer.normalized, distanceToPlayer, config.obstacleLayer))
            {
                //if there are obstacles in the way return false
                return false;
            }

            return true;
        }

        private void Start()
        {
            Player = GameObject.FindWithTag("Player");
        }
    }
}