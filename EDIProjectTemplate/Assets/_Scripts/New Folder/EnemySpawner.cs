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
    [SerializeField] AiAgent enemyPrefab;
    [SerializeField] int spawnCount = 10;
    [SerializeField] float spawnRadius = 10f;
    [SerializeField] float spawnInterval = 3f;
    
    public float navMeshSampleDistance = 2f;

    private void Start()
    {
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
            InvokeRepeating(nameof(SpawnOne), 0f, spawnInterval);
        }
    }

    void SpawnBatch()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnOne();
        }
    }

    void SpawnOne()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPoint.y = transform.position.y;

        Vector3 spawnPosition = randomPoint;


        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }
        else
        {
            return;
        }

        var aiAgent = container.InstantiatePrefabForComponent<AiAgent>(enemyPrefab, spawnPosition, quaternion.identity, transform);
        enemyManager.RegisterEnemy(aiAgent);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}