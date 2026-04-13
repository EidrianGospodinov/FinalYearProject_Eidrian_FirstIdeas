using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.New_Folder.Spawner
{
    public class Area : MonoBehaviour
    {
        public float Radius = 20f;
        private Vector3 areaCentre;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(areaCentre, Radius);
        }

        public Vector3 GetRandomPoint(Vector3 terrainCentre)
        {
            areaCentre = terrainCentre;
            Vector3 randomDirection = Random.insideUnitSphere * Radius;
            randomDirection.y = 0f;

            Vector3 randomPoint = areaCentre + randomDirection;

            NavMeshHit hit;
            Vector3 finalPosition = areaCentre;

            if (NavMesh. SamplePosition(randomPoint, out hit, 2f, 1))
            {
                finalPosition = hit.position;
            }

            return finalPosition;
        }
    }
}