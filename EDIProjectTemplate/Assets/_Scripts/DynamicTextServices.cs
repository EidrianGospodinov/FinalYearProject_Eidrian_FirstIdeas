using _Scripts.Units.Enemy;
using UnityEngine;
using Zenject;

namespace _Scripts
{
    public class DynamicTextServices
    {
        public float HandleDamageVisuals(Transform transform ,Collider other, AiAgent agent, float damageTaken, bool applyCrit = false)
        {
            var agentConfig = agent.agentConfig;
            DynamicTextData data = agentConfig.DynamicTextData;
            Vector3 surfacePoint = other.ClosestPoint(transform.position);
            float offsetDistance = 0.5f; 
            Vector3 dirToPlayer = (transform.position - surfacePoint).normalized;
            Vector3 destination = surfacePoint + (dirToPlayer * offsetDistance);

            if (applyCrit)
            {
                HandleCritLogic(ref damageTaken, agentConfig, destination);
            }

            DynamicTextManager.CreateText(destination, damageTaken.ToString(), data);
            return damageTaken;
        }

        private void HandleCritLogic(ref float damageTaken, AiAgentConfig agentConfig, Vector3 destination)
        {
            float roll = UnityEngine.Random.value;
            if (agentConfig.critChance > 0 && roll <= agentConfig.critChance)
            {
                DynamicTextManager.CreateText(destination + Vector3.up, "CRIT!", agentConfig.CritData);
                damageTaken *= 1.5f;
            }
        }
    }
}