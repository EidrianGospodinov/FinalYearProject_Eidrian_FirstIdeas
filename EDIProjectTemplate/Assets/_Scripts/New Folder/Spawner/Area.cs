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
            int maxAttempts = 10;

            for (int i = 0; i < maxAttempts; i++)
            {
                areaCentre = terrainCentre;
                /*Vector3 randomDirection = Random.insideUnitSphere * Radius;
                randomDirection.y = 0f;

                Vector3 randomPoint = areaCentre + randomDirection;*/
                Vector2 randomPoint2D = Random.insideUnitCircle * Radius;

                Vector3 randomPoint = new Vector3(
                    terrainCentre.x + randomPoint2D.x,
                    terrainCentre.y,
                    terrainCentre.z + randomPoint2D.y
                );
                if (NavMesh.SamplePosition(randomPoint, out var hit, 2f, NavMesh.AllAreas))
                {
                    return hit.position;
                }

            }
            return terrainCentre + (Random.insideUnitSphere * 2f);
            
        }
    }
}