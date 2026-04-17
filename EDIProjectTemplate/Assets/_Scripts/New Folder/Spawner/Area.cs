using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _Scripts.New_Folder.Spawner
{
    public class Area : MonoBehaviour
    {
        public float Radius = 20f;
        private Vector3 areaCentre;
        private List<Vector3> debugFailedPoints = new List<Vector3>();
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(areaCentre, Radius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(areaCentre, 1);
            
            
            Gizmos.color = Color.yellow;
            foreach (Vector3 point in debugFailedPoints)
            {
                Gizmos.DrawSphere(point, 0.5f);
                Gizmos.DrawLine(point, areaCentre);
            }
        }
        

        public Vector3 GetRandomPoint(Vector3 terrainCentre)
        {
            //debugFailedPoints.Clear();
            int maxAttempts = 30;
            areaCentre = terrainCentre;
            Vector3 skyOrigin = areaCentre + Vector3.up * 50f;
            
            if(Physics.Raycast(skyOrigin, Vector3.down, out RaycastHit firstHit, 100f, LayerMask.GetMask("Terrain")))
            {
                areaCentre = firstHit.point;
            }
            
            NavMeshPath path = new NavMeshPath();
            for (int i = 0; i < maxAttempts; i++)
            {
                /*Vector3 randomDirection = Random.insideUnitSphere * Radius;
                randomDirection.y = 0f;

                Vector3 randomPoint = areaCentre + randomDirection;*/
                Vector2 randomPoint2D = Random.insideUnitCircle * Radius;

                Vector3 randomPoint = new Vector3(
                    terrainCentre.x + randomPoint2D.x,
                    terrainCentre.y,
                    terrainCentre.z + randomPoint2D.y
                );
                if (NavMesh.SamplePosition(randomPoint, out var hit, 5f, NavMesh.AllAreas))
                {
                    if (NavMesh.CalculatePath(hit.position, areaCentre, NavMesh.AllAreas, path))
                    {
                        //if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            return hit.position;
                        }
                        
                    }

                    debugFailedPoints.Add(hit.position);
                }


            }
            Debug.LogError("Couldn't get path, going teleporting at centre");
            return areaCentre + (Random.insideUnitSphere * 2f);
            
        }
    }
}