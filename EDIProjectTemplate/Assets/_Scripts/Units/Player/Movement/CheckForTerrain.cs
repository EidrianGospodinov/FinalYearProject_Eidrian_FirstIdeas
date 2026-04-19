using System;
using _Scripts.Units.Sound.Footstep;
using UnityEngine;

public class CheckForTerrain : MonoBehaviour
{
    private EnemySpawner previousEnemySpawner;
    private EnemySpawner currentEnemySpawner;
    private TerrainChecker checker;

    private PlayerController playerController;

    private string currentLayer;
    public FootstepCollection[] terrainFootstepCollection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checker = new TerrainChecker();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = RayToGround();
        if (hit.collider == null)
        {
            return;
        }
        var newTerrainSpawner = hit.collider.GetComponent<EnemySpawner>();

        if (newTerrainSpawner == null)
        {
            return;
        }

        if (newTerrainSpawner == currentEnemySpawner)
        {
            return;
        }

        if (previousEnemySpawner != null)
        {
            previousEnemySpawner.DestroyAllEnemies();
            //possible delete the enemies from the previous spawner
        }

        previousEnemySpawner = this.currentEnemySpawner;
        currentEnemySpawner = newTerrainSpawner;

        newTerrainSpawner.SpawnEnemies();


    }

    private RaycastHit RayToGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            return hit;
        }

        return default;
    }
    public void CheckLayers()
    {
        RaycastHit hit = RayToGround();
        if (hit.collider == null)
        {
            return;
        }
        Terrain t = hit.collider.GetComponent<Terrain>();
        if (t == null)
        {
            return;
        }

        string layerName = checker.GetLayerName(transform.position, t);
        if (currentLayer != layerName )
        {
            currentLayer = layerName;
            foreach (var collection in terrainFootstepCollection)
            {
                if (currentLayer == collection.name)
                {
                    playerController.SwapCollection(collection);
                }
            }
        }
    }
    
}
