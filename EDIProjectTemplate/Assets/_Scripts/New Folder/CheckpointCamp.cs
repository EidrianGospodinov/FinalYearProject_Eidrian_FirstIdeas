using System;
using UnityEngine;

public class CheckpointCamp : MonoBehaviour
{
    [SerializeField] private CampFire campFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActiveCheckpoint(bool isActive)
    {
        campFire.ActivateFire(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        SetActiveCheckpoint(true);
    }
}
