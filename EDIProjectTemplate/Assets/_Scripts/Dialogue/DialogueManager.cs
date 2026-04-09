using System;
using _Scripts.Units.Player.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Dialogue
{
    public abstract class DialogueManager : MonoBehaviour, IInteractable
    {
        [Inject] private GameManager gameManager; 
        [SerializeField] protected GameObject dialogueUI;
        
        
        [Header("DialogueUi prefab")]
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private RawImage interactImage;
        
        private float distance;
        private bool isTalking = false;
        public bool IsTalking => isTalking;
        
        protected virtual void Start()
        {
            dialogueUI.SetActive(false);
        }
        /*public virtual void OnMouseOver()
            {
                distance = Vector3.Distance(player.transform.position, this.transform.position);
                if (distance <= 2.5f)
                {
                    if (!isTalking)
                    {
                        interactText.gameObject.SetActive(true);
                        interactImage.gameObject.SetActive(false);
        
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            interactText.gameObject.SetActive(false);
                            StartConversation();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        EndConversation();
                    }
                }
                else 
                {
                    if (isTalking)
                    {
                        EndConversation();
                    }
                }
                
            }*/
        
            private void OnMouseExit()
            {
                interactText.gameObject.SetActive(false);
                interactImage.gameObject.SetActive(true);
            }
            protected virtual void EndConversation()
            {
                isTalking = false;
                gameManager.SetGameState(GameState.InGame);
                Invoke(nameof(DisablePanel), 3f);
            }
            void DisablePanel()
            {
                dialogueUI.SetActive(false);
            }

            protected virtual void StartConversation()
            {
                isTalking = true;
                dialogueUI.SetActive(true);
                gameManager.SetGameState(GameState.InDialogue);

            }

            public void Interact()
            {
                StartConversation();
            }
    }
}