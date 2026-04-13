using System;
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
    private AiAgent enemyPrefab;
    [SerializeField] int spawnCount = 10;
    [SerializeField] private int minSpawnCountBeforeWave = 3;
    [SerializeField] float spawnInterval = 0;

    private Vector3 terrainCentre;
    private Terrain terrain;
    private int currentSpawnCount = 0;

    private void Start()
    {
        area = GetComponent<Area>();
        //SpawnEnemies();
    }

    public void SetUpSpawner(Terrain terrain, AiAgent aiAgent)
    {
        if (terrain != null && aiAgent != null)
        {
            enemyPrefab = aiAgent;
            this.terrain = terrain;
            // Calculate the center based on terrain dimensions
            Vector3 size = terrain.terrainData.size;
            terrainCentre = transform.position + new Vector3(size.x / 2f, 0, size.z / 2f);
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
        var randomPoint = area.GetRandomPoint(terrainCentre);
        var aiAgent = container.InstantiatePrefabForComponent<AiAgent>(enemyPrefab, randomPoint, quaternion.identity, transform);
        enemyManager.RegisterEnemy(aiAgent);
        aiAgent.AssignSpawner(this);
        currentSpawnCount++;

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
            Debug.Log($"On deat in {this.name}. Spawning new wave");
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