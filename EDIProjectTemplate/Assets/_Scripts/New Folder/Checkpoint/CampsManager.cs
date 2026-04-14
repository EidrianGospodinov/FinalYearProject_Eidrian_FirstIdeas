using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.New_Folder.Checkpoint;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class CampsManager : MonoBehaviour
{
    private RespawnService respawnService;
    private DiContainer diContainer;
    private EnemyManager enemyManager;
    private TeleporterService teleporterService;
    private ICameraService cameraService;

    [Inject]
    public void Construct(
        DiContainer diContainer,
        RespawnService respawnService,
        EnemyManager enemyManager,
        TeleporterService teleporterService,
        ICameraService cameraService)
    {
        this.respawnService = respawnService;
        this.diContainer = diContainer;
        this.enemyManager = enemyManager;
        this.teleporterService = teleporterService;
        this.cameraService = cameraService;

    }
    
    [SerializeField] private PlayerController playerControllerPrefab;
    private PlayerController playerControllerInstance;
    private List<CheckpointCamp> checkpointCamps = new List<CheckpointCamp>();

    private CheckpointCamp currentActiveCheckpointCamp;

    private EventBinding<OnCheckpointEnter> OnCheckpointEnter;
    private EventBinding<OnCheckpointExit> OnCheckpointExit;
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
        OnCheckpointExit = EventBus<OnCheckpointExit>.Register(OnCheckpointExitEvent);

    }


    private void OnCheckpointEnterEvent(OnCheckpointEnter checkpointEnter)
    {
        if (!respawnService.IsActiveCheckpoint(checkpointEnter.EnteredCheckpoint))
        {
            if (currentActiveCheckpointCamp != null)
            {
                currentActiveCheckpointCamp.SetActiveCheckpoint(false);
            }
            currentActiveCheckpointCamp = checkpointEnter.EnteredCheckpoint;
            respawnService.SetCheckpoint(currentActiveCheckpointCamp);
            teleporterService.RegisterCamp(currentActiveCheckpointCamp);
        }
        enemyManager.SetEnemiesInSafeZone(true);
    }
    private void OnCheckpointExitEvent(OnCheckpointExit obj)
    {
        enemyManager.SetEnemiesInSafeZone(false);
    }

    private void MakeTheCamFollowPlayer()
    {
        cameraService.SetTarget(playerControllerInstance.GetCameraFollowOffset);
        /*var cineBrain = Camera.main.GetComponent<CinemachineBrain>();
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
        }*/
    }

    private void PlacePlayerInCamp()
    {
        if (teleporterService.GetDiscoveredCamps().Count > 0)
        {
            teleporterService.GetDiscoveredCamps()[0].PlacePlayerInCamp(playerControllerInstance);
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
                teleporterService.RegisterCamp(checkpoint);
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
        EventBus<OnCheckpointExit>.Unregister(OnCheckpointExit);
    }
}