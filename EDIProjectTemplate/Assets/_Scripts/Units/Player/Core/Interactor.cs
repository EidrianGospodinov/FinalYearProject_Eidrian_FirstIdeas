using System;
using TMPro;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    public interface IInteractable
    {
        void Interact();
    }
    public class Interactor : MonoBehaviour
    {
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
         //   Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Ray ray = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
            {
                print("interactor ray hit: " + hit.collider.name);
                if (hit.collider.gameObject.TryGetComponent(out IInteractable interactableObject))
                {
                    return interactableObject;
                }
            }

            return null;
        }
    }
}