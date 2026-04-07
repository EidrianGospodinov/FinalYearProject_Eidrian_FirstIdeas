using System;
using System.Collections.Generic;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private AiAgent AiAgent;
    [Inject]
    private DiContainer _container;

    [Inject] private EnemyManager enemyManager;

    public void Interact()
    {
        var aiAgent = _container.InstantiatePrefabForComponent<AiAgent>(AiAgent, transform);
        enemyManager.RegisterEnemy(aiAgent);
        //Instantiate(AiAgent, transform.position, transform.rotation);
    }
}
