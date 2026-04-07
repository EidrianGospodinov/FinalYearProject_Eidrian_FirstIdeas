using _Scripts.New_Folder.Checkpoint;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class TentTeleporter : MonoBehaviour, IInteractable
{
    [Inject] private TeleporterService teleporterService;

    [SerializeField] private MapTeleporter mapTeleporter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        mapTeleporter.gameObject.SetActive(true);
    }
}
