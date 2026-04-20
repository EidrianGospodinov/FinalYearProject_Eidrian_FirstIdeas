using _Scripts.StateMachine.EnemyStatemMachine;
using _Scripts.Units.Player.Core;
using TMPro;
using UnityEngine;

namespace _Scripts.Units.Enemy
{
    public class AiVision : MonoBehaviour
    {
        [HideInInspector] public GameObject Player;

        public bool IsPlayerDetected(AiAgent agent, bool angleDoesntMatter, float distMultiplier) {
            if (agent.GameManager.GetCurrentGameState != GameState.InGame)
            {
                return false;
            }
            return IsPlayerDetected(agent.agentConfig, angleDoesntMatter,  distMultiplier);
        }


        private bool IsPlayerDetected(AiAgentConfig config, bool angleDoesntMatter, float distMultiplier)
        {
            if (Player == null)
            {
                return false;
            }
            Vector3 directionToPlayer = Player.transform.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer > (config.maxSightDistance * distMultiplier))
            {
                return false;
            }


            Vector3 eyePosition = transform.position + Vector3.up * 1f;
            if (Physics.Raycast(eyePosition, directionToPlayer.normalized, distanceToPlayer, config.obstacleLayer))
            {
                //if there are obstacles in the way return false
                return false;
            }

            if (angleDoesntMatter)
            {
                return true;
            }
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer > config.angleVision / 2f)
            {
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