using System.Threading.Tasks;
using _Scripts.Units.Player.Core;
using UnityEngine;
using UnityEngine.AI;

namespace _Scripts.Commands
{
    public interface ICommand
    {
        Task<Transform>  Execute(NPCController npc);
        Transform target { get; set; }
        //bool IsFinished { get; }
    }
    public class MoveToCommand : ICommand
    {
        private float _stoppingDistance = 0.25f;
        
        //public bool IsFinished { get; private set; }
        public MoveToCommand(Transform target) => this.target = target;
        public async Task<Transform> Execute(NPCController npc)
        {
            NavMeshAgent agent = npc.Agent;
            if (agent == null)
            {
                Debug.LogError($"No NavMeshAgent found on {npc.name}");
                return null;
            }

            agent.isStopped = false;
            agent.SetDestination(target.position);

            
            await Task.Yield();
            while (agent.pathPending || agent.remainingDistance > _stoppingDistance)
            {
                await Task.Yield();
            }
            //IsFinished = true;
            return null;
        }

        public Transform target { get; set; }

        private System.Collections.IEnumerator WaitForArrival(NavMeshAgent agent)
        {
            // Wait for a frame to let the path calculate
            yield return null;

            // Loop until the agent is close enough
            while (agent.pathPending || agent.remainingDistance > _stoppingDistance)
            {
                yield return null; 
            }
            
            //IsFinished = true;
        }

    }

    public class InteractCommand : ICommand
    {
        public InteractCommand(Transform target) => this.target = target;
        //public bool IsFinished { get; private set; }

        public async Task<Transform> Execute(NPCController npc)
        {
            var interactable = target.GetComponent<IInteractable>();
            interactable.Interact();
            if (interactable is IResultInteractable resultInteractable)
            {
                var result = await resultInteractable.GetResult();
                if (result != null)
                {
                    return result;
                }
                //TryGetResult(resultInteractable);
                // use result
            }
            return null;
        }

        public Transform target { get; set; }

        private async void TryGetResult(IResultInteractable interactable)
        {
            var result = await interactable.GetResult();
            if (result != null)
            {
                
            }
        }
    }
    public class GiveCommand : ICommand
    {
        private NPCController npcController;
        public GiveCommand(Transform target, NPCController npcController)
        {
            this.target = target;
            this.npcController = npcController;
        }
        public Task<Transform> Execute(NPCController npc)
        {
            if (target == null)
            {
                Debug.LogError($"No target found on {npc.name}");
                return null;
            }

            var desiredItem = npcController.GetDesiredItem();
            if (desiredItem == null)
            {
                Debug.LogError($"No desired item found on {npc.name}");
                return null;
            }
            var receiveItemClass = target.GetComponent<ReceiveItem>();
            if (receiveItemClass != null)
            {
                receiveItemClass.SetReceiveTransform(npcController.GetDesiredItem());
                return null;
            }

            Debug.LogError($"No ReceiveItem found on {target.name}");
            return null;
        }

        public Transform target { get; set; }
    }
}
