using System;
using System.Collections.Generic;
using _Scripts.Units.Player.Combat;
using _Scripts.Units.Player.Core;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Dialogue
{
    public abstract class DialogueManager : MonoBehaviour, IInteractable
    {
        [Inject] private GameManager gameManager;
        [Inject] private QuestManager questManager;
        [SerializeField] protected GameObject dialogueUI;
        
        
        [Header("DialogueUi prefab")]
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private RawImage interactImage;
        
        [FormerlySerializedAs("goal")]
        [Header("Optional- if the camp has a dialgue")]
        [SerializeField] private SingleEnemySpawner enemySpawner;

        [SerializeField] QuestScriptable quest;
        private bool hasEnemySpawner = false;
        
        private float distance;
        private bool isTalking = false;
        private bool firstTimeTalking = true;
        public bool IsTalking => isTalking;
        
        protected virtual void Start()
        {
            dialogueUI.SetActive(false);
            hasEnemySpawner = enemySpawner != null;
        }
        
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
                /*List<Objective> objectives = new List<Objective>
                {
                    new KillEnemyObjective("Kill enemies: ",1),
                    new FindObjectObjective("Find sword", SearchItemType.Sword)
                };
                questManager.SetQuest(new Quest(objectives));*/
                questManager.SetQuest(new Quest(quest.objectives));
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

                if (firstTimeTalking && hasEnemySpawner)
                {
                   EventBus<OnItemFound>.Trigger(new OnItemFound(SearchItemType.Guide));
                   enemySpawner.gameObject.SetActive(true);
                }

                firstTimeTalking = false;
            }

            public void Interact()
            {
                StartConversation();
            }
    }
}