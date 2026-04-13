using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Core;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class AutomatedTerrainGenerator : MonoBehaviour
{
    [Inject] private GameObjectSpawner_DI gameObjectSpawnerDi;
    [SerializeField] private AiAgent enemyPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var terrains = GetComponentsInChildren<Terrain>();
        foreach (var terrain in terrains)
        {
            var spawner = gameObjectSpawnerDi.AddComponent<EnemySpawner>(terrain.gameObject);
            spawner.SetUpSpawner(terrain, enemyPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
