using System;
using System.Collections.Generic;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Core;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private AiAgent AiAgent;

    public void Interact()
    {
        Instantiate(AiAgent, transform.position, transform.rotation);
    }
}
