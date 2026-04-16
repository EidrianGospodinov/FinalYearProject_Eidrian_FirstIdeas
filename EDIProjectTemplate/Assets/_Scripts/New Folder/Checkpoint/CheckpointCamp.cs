using System;
using System.Collections.Generic;
using _Scripts.Dialogue;
using _Scripts.Units.Player.Combat;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class CheckpointCamp : MonoBehaviour
{
    [Inject] private QuestManager questManager;
    [SerializeField] private string name;
    [SerializeField] private CampFire campFire;
    [SerializeField] private Transform spawnerLocation;
    
    [Header("Optional- if the camp has a dialgue")]
    [SerializeField] private MultipleChoiceDialogueManager guide;

    private bool hasGuide = false;
    public bool IsCampDiscovered { get; private set; }
    private PlayerController playerController;
    

    public string GetName => name;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasGuide = guide != null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlacePlayerInCamp(PlayerController _playerController = null)
    {
        if (playerController == null && _playerController != null)
        {
            playerController = _playerController;
        }
        
        var controller = this.playerController.GetComponent<CharacterController>();
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
            if (IsCampDiscovered == false && hasGuide)
            {
                List<Objective> objectives = new List<Objective>
                {
                    new FindObjectObjective($"Find the Guide: {guide.name} and speak with him", SearchItemType.Guide)//temp name of gameobject
                };
                questManager.SetQuest(new Quest(objectives));
                guide.gameObject.SetActive(true);
            }
            if (playerController == null)
            {
                playerController = other.gameObject.GetComponent<PlayerController>();
            }
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
