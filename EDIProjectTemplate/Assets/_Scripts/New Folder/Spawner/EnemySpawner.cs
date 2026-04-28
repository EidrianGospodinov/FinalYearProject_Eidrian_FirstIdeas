using System;
using System.Collections.Generic;
using _Scripts.New_Folder.Spawner;
using _Scripts.Units.Enemy;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Area))]
public class EnemySpawner : MonoBehaviour
{
    [Inject] private DiContainer container;
    [Inject] private EnemyManager enemyManager;

    [Header("Spawn Settings")] 
    private Area area;
    private List<AiAgent> enemyPrefabs;
    [SerializeField] int spawnCount = 10;
    [SerializeField] private int minSpawnCountBeforeWave = 3;
    [SerializeField] float spawnInterval = 1.4f;

    private Vector3 terrainCentre;
    private Terrain terrain;
    private int currentSpawnCount = 0;

    private void Awake()
    {
        area = GetComponent<Area>();
        //SpawnEnemies();
    }

    public void SetUpSpawner(Terrain terrain, List<AiAgent> aiAgent)
    {
        if (terrain != null && aiAgent != null)
        {
            enemyPrefabs = aiAgent;
            this.terrain = terrain;
            // Calculate the center based on terrain dimensions
            Vector3 size = terrain.terrainData.size;
            terrainCentre = transform.position + new Vector3(size.x / 2f, 0, size.z / 2f);
            
            //a^2 + b^2 = c^2
            area.Radius = Mathf.Sqrt((float)(Math.Pow(size.x / 2f, 2) + Math.Pow(size.z / 2f, 2)));
        }
        
    }

    public void SpawnEnemies()
    {
        if (spawnInterval <= 0f)
        {
            SpawnBatch();
        }
        else
        {
            InvokeRepeating(nameof(SpawnOneByOne), 0f, spawnInterval);
        }
    }

    public void DestroyAllEnemies()
    {
        enemyManager.DestroyAllEnemiesFromArea(this);
    }

    void SpawnBatch()
    {
        var enemiesToSpawn = spawnCount - currentSpawnCount;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnOne();
        }
    }

    void SpawnOneByOne()
    {
        if (currentSpawnCount >= spawnCount)
        {
            CancelInvoke(nameof(SpawnOneByOne));
            return;
        }

        SpawnOne();
    }

    void SpawnOne()
    {
        if (enemyPrefabs == null)
        {
            Debug.LogError("No enemy prefabs to spawn");
            return;
        }
        int prefabIndex = GetEnemyPrefabIndex();
        var randomPoint = area.GetRandomPoint(terrainCentre);
        var aiAgent = container.InstantiatePrefabForComponent<AiAgent>(enemyPrefabs[prefabIndex],
            randomPoint, quaternion.identity, transform);
        enemyManager.RegisterEnemy(aiAgent);
        aiAgent.AssignSpawner(this);
        currentSpawnCount++;

    }
    int GetEnemyPrefabIndex()
    {
        if (!enemyManager.FinalQuestCompleted)
        {
            return 0;
        }

        if (enemyPrefabs.Count <= 1)
        {
            return 0;
        }
        float roll = UnityEngine.Random.value;
        //80% of the time it should spawn the boar
        if (roll < 0.8f)
        {
            return 0;
        }
        // get a random enemy of the rest 
        return UnityEngine.Random.Range(1, enemyPrefabs.Count);
    }
    public void NotifyDeath(bool destroy)
    {
        currentSpawnCount--;
        if (destroy)
        {
            return;
        }
        if (currentSpawnCount <= minSpawnCountBeforeWave)
        {
            Debug.Log($"On death in {this.name}. Spawning new wave");
            SpawnEnemies();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEnemies();
        }
    }
}