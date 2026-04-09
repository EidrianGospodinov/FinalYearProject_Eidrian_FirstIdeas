using System;
using System.Collections.Generic;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class SingleEnemySpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private AiAgent AiAgent;
    [Inject]
    private DiContainer container;

    [Inject] private EnemyManager enemyManager;

    public void Interact()
    {
        var aiAgent = container.InstantiatePrefabForComponent<AiAgent>(AiAgent, transform);
        enemyManager.RegisterEnemy(aiAgent);
        //Instantiate(AiAgent, transform.position, transform.rotation);
    }
}
