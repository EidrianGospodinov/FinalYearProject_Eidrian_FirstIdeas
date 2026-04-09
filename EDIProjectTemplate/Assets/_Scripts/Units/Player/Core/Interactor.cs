using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.Units.Player.Core
{
    public interface IInteractable
    {
        void Interact();
    }
    public interface IResultInteractable : IInteractable
    {
        Task<Transform> GetResult();
    }
    public class Interactor : MonoBehaviour
    {
        [Inject] private GameManager gameManager;
        
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private float interactRange;
        private Transform interactorSource;

        private void Start()
        {
            interactorSource = Camera.main.transform;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var interactableObject = GetInteractableObject();
                if (interactableObject != null)
                {
                    interactableObject.Interact();
                }
            }
        }

        public IInteractable GetInteractableObject()
        {
            if (gameManager.GetCurrentGameState != GameState.InGame)
            {
                return null;
            }
            //   Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Ray ray = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                //print("interactor ray hit: " + hit.collider.name);
                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactableObject))
                {
                    return interactableObject;
                }
            }

            return null;
        }
    }
}