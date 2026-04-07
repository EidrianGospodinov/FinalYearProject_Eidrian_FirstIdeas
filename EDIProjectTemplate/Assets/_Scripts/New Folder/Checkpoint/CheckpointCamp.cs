using System;
using UnityEngine;

public class CheckpointCamp : MonoBehaviour
{
    [SerializeField] private CampFire campFire;
    [SerializeField] private Transform spawnerLocation;
    public bool IsCampDiscovered { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePlayerInCamp(PlayerController playerController)
    {
        var controller = playerController.GetComponent<CharacterController>();
        controller.enabled = false;
        playerController.gameObject.transform.position = spawnerLocation.transform.position;
        playerController.gameObject.transform.rotation = spawnerLocation.transform.rotation;
        controller.enabled = true;
    }
    public void SetActiveCheckpoint(bool isActive)
    {
        campFire.ActivateFire(isActive);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsCampDiscovered = true;
            SetActiveCheckpoint(true);
            EventBus<OnCheckpointEnter>.Trigger(new OnCheckpointEnter(this));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventBus<OnCheckpointExit>.Trigger(new OnCheckpointExit());
        }
    }
}

public class OnCheckpointEnter : IEvent
{
    public CheckpointCamp EnteredCheckpoint { get; private set; }

    public OnCheckpointEnter(CheckpointCamp enteredCheckpoint)
    {
        EnteredCheckpoint = enteredCheckpoint;
    }
}

public class OnCheckpointExit : IEvent
{
    
}
