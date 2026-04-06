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
    public void Interact()
    {
        _container.InstantiatePrefab(AiAgent, transform);
        //Instantiate(AiAgent, transform.position, transform.rotation);
    }
}
