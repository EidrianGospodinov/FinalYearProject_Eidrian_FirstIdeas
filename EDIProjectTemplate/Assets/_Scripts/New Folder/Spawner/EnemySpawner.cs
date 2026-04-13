using _Scripts.New_Folder.Spawner;
using _Scripts.Units.Enemy;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Inject] private DiContainer container;
    [Inject] private EnemyManager enemyManager;

    [Header("Spawn Settings")] 
    [SerializeField] private Area area;
    [SerializeField] private AiAgent enemyPrefab;
    [SerializeField] int spawnCount = 10;
    [SerializeField] float spawnInterval = 3f;
    
    
    private int currentSpawnCount = 0;

    private void Start()
    {
        area = GetComponent<Area>();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (spawnInterval <= 0f)
        {
            SpawnBatch();
        }
        else
        {
            currentSpawnCount = 0;
            InvokeRepeating(nameof(SpawnOneByOne), 0f, spawnInterval);
        }
    }

    void SpawnBatch()
    {
        for (int i = 0; i < spawnCount; i++)
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
        var randomPoint = area.GetRandomPoint();
        var aiAgent =
            container.InstantiatePrefabForComponent<AiAgent>(enemyPrefab, randomPoint, quaternion.identity, transform);
        enemyManager.RegisterEnemy(aiAgent);
        aiAgent.AssignSpawner(this);
        currentSpawnCount++;

    }
    public void NotifyDeath() => currentSpawnCount--;
}