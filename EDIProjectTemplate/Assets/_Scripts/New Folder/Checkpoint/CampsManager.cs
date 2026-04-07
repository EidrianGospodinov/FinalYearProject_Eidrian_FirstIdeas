using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class CampsManager : MonoBehaviour
{
    [Inject] private RespawnService respawnService;
    [Inject] private DiContainer diContainer;
    
    [SerializeField] private PlayerController playerControllerPrefab;
    private PlayerController playerControllerInstance;
    private List<CheckpointCamp> checkpointCamps = new List<CheckpointCamp>();
    private List<CheckpointCamp> discoveredCheckpointCamps = new List<CheckpointCamp>();

    private CheckpointCamp currentActiveCheckpointCamp;

    private EventBinding<OnCheckpointEnter> OnCheckpointEnter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerInstance = diContainer.InstantiatePrefabForComponent<PlayerController>(playerControllerPrefab);
        checkpointCamps = GetComponentsInChildren<CheckpointCamp>().ToList();
        if (checkpointCamps.Count == 0)
        {
            Debug.LogError("No checkpoints detected");
            return;
        }
        SetupCheckpoints();
        PlacePlayerInCamp();
        MakeTheCamFollowPlayer();
        OnCheckpointEnter = EventBus<OnCheckpointEnter>.Register(OnCheckpointEnterEvent);
    }

    private void OnCheckpointEnterEvent(OnCheckpointEnter checkpointEnter)
    {
        currentActiveCheckpointCamp = checkpointEnter.EnteredCheckpoint;
        respawnService.SetCheckpoint(currentActiveCheckpointCamp);
    }

    private void MakeTheCamFollowPlayer()
    {
        var cineBrain = Camera.main.GetComponent<CinemachineBrain>();
        CinemachineCamera cineCam = null;
        if (cineBrain != null)
        {
            cineCam = cineBrain.ActiveVirtualCamera as CinemachineCamera;
            if (cineCam != null)
            {
                cineCam.Follow = playerControllerInstance.GetCameraFollowOffset;
            }
        }

        if (!cineBrain || !cineCam)
        {
            Debug.LogError("Camera does not follow the player");
        }
    }

    private void PlacePlayerInCamp()
    {
        if (discoveredCheckpointCamps.Count > 0)
        {
            discoveredCheckpointCamps[0].PlacePlayerInCamp(playerControllerInstance);
        }
        else
        {
            checkpointCamps[0].PlacePlayerInCamp(playerControllerInstance);
        }
    }

    private void SetupCheckpoints()
    {
        foreach (var checkpoint in checkpointCamps)
        {
            if (checkpoint.IsCampDiscovered)
            {
                discoveredCheckpointCamps.Add(checkpoint);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventBus<OnCheckpointEnter>.Unregister(OnCheckpointEnter);
    }
}