using System;
using System.Collections.Generic;
using _Scripts.Units.Enemy;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private AiAgent AiAgent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
             Instantiate(AiAgent, transform.position, transform.rotation);
    }
}
