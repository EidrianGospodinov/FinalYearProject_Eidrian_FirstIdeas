using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Commands;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public interface ICommandReceiver
{
    void ExecuteCommand(string commandID, Item nodeDesiredItem);
}
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NPCController : MonoBehaviour, ICommandReceiver
{
    public NavMeshAgent Agent { get; private set; }
    public Animator Anim { get; private set; }

    private Queue<ICommand> commandQueue = new Queue<ICommand>();

    private ICommand currentCommand;
    
    [Header("Positions")]
    [SerializeField] Transform stoveTransform;

    [SerializeField] Transform counterTransform;
    [SerializeField] Transform ItemPlaceHolder;
    private Item currentDesiredItem;
    private Transform desiredItemTransform;
    private Transform currentTargetTransform;
    private Transform playerTransform;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("No player found");
        }
    }

    public Transform GetDesiredItem() => desiredItemTransform;

    public void AddTask(ICommand command)
    {
        commandQueue.Enqueue(command);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCommand == null)
        {
            if (commandQueue.Count > 0)
            {
                currentCommand = commandQueue.Dequeue();
                currentTargetTransform = currentCommand.target;// set target transform
                print("executing command: " + currentCommand);
                TryGetResult();
            }
            else
            {
                currentTargetTransform = null;
            }
        }
        /*else
        {
            if (currentCommand.IsFinished)
            {
                currentCommand = null;
            }
        }*/
    }

    private async Task TryGetResult()
    {
        var result = await currentCommand.Execute(this);
        print("we received result: " + result);
        if (result != null)
        {
            if (currentDesiredItem != null)
            {
                if (result.GetComponent<Item>().ItemName == currentDesiredItem.ItemName)
                {
                    desiredItemTransform = result;
                    desiredItemTransform.SetParent(ItemPlaceHolder);
                    desiredItemTransform.SetLocalPositionAndRotation(Vector3.zero, quaternion.identity);
                }
            }
        }

        currentCommand = null;
    }

    public void ExecuteCommand(string commandID, Item nodeDesiredItem)
    {
        currentDesiredItem = nodeDesiredItem;
        commandQueue.Clear();
        currentCommand = null;
        switch (commandID)
        {
            case "Cook_Pizza":
                AddTask(new MoveToCommand(stoveTransform));
                AddTask(new InteractCommand(stoveTransform));
                AddTask(new MoveToCommand(counterTransform));
                
                
                AddTask(new GiveCommand(playerTransform, this));
                break;
        }
    }
    private void LateUpdate()
    {
        if (currentTargetTransform == null)
        {
            currentTargetTransform = Camera.main.transform;
        }
        Vector3 targetDir = currentTargetTransform.position - transform.position;
        targetDir.y = 0; 

        transform.rotation = Quaternion.LookRotation(targetDir);
        
    }
}
