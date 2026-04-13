using System;
using UnityEngine;

public class CheckForTerrain : MonoBehaviour
{
    private EnemySpawner previousEnemySpawner;
    private EnemySpawner currentEnemySpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            var terrainSpawner = hit.collider.GetComponent<EnemySpawner>();

            if (terrainSpawner == null)
            {
                return;
            }
            if (terrainSpawner == currentEnemySpawner)
            {
                return; 
            }
            if (previousEnemySpawner != null)
            {
                //possible delete the enemies from the previous spawner
            }
            previousEnemySpawner = this.currentEnemySpawner;
            currentEnemySpawner = terrainSpawner;

            terrainSpawner.SpawnEnemies();

        }
    }
    
}
