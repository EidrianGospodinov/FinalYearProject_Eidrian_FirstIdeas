using System;
using System.Collections.Generic;
using _Scripts.New_Folder.Spawner;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Combat;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class SingleEnemySpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private AiAgent AiAgent;
    [SerializeField] private Transform enemySpawnTransform;
    [SerializeField] private GameObject beam;
    private Area area;
    [Inject]
    private DiContainer container;

    [Inject] private EnemyManager enemyManager;

    private void Start()
    {
        if (enemySpawnTransform == null)
        {
            enemySpawnTransform = transform;
        }
    }

    public void Interact()
    {
        EventBus<OnItemFound>.Trigger(new OnItemFound(SearchItemType.EnemySpawner));
        var aiAgent = container.InstantiatePrefabForComponent<AiAgent>(AiAgent, enemySpawnTransform);
        enemyManager.RegisterEnemy(aiAgent);
        beam.SetActive(false);
        //Instantiate(AiAgent, transform.position, transform.rotation);
    }
}
