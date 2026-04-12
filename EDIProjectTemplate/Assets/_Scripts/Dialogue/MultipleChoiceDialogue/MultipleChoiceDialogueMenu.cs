using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Dialogue.ScriptableDialogue;
using TMPro;
using UnityEngine;

namespace _Scripts.Dialogue.MultipleChoiceDialogue
{
    public class MultipleChoiceDialogueMenu : MonoBehaviour
    {
        enum DialogueType
        {
            None,
            Linear,
            MultiChoice
            
        }
        [SerializeField] private GameObject optionsPanel;
        [SerializeField] private ChoiseDialogue[] options = new ChoiseDialogue[3];

        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private DialogueNodeBase fallBackDialogue;

        private string npcName = " ";
        private Choice[] choices;
        private int currentResponseTracker;
        
         private EventHandler OnDialogueFinished { get; set; }
         private ICommandReceiver commandReceiver;
         private DialogueType currentDialogueType = DialogueType.None;
         private int taskDelayMilSecDefault = 2500;
         private int taskDelayMilSec = 2500;

        private void OnEnable()
        {
            currentResponseTracker = 0;
            options[currentResponseTracker].EnableHighlight();
        }

        private void OnDisable()
        {
            DisableHighlights();
            choices = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndDialogue();
            }

            if (currentDialogueType == DialogueType.Linear)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    taskDelayMilSec = 0;
                }
            }
            if (currentDialogueType != DialogueType.MultiChoice)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (choices != null)
                {
                    var nextNode = choices[currentResponseTracker].nextNode;
                    if (nextNode != null)
                    {
                        SetUpDialogue(nextNode);
                    }
                    else
                    {
                        currentDialogueType = DialogueType.None;
                        SetUpDialogue(fallBackDialogue);
                        Debug.Log("Next node is empty");
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentResponseTracker++;
                if (currentResponseTracker > options.Length - 1)
                {
                    currentResponseTracker = options.Length - 1;
                }
                else
                {
                    DisableHighlights();
                    options[currentResponseTracker].EnableHighlight();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentResponseTracker--;
                if (currentResponseTracker < 0)
                {
                    currentResponseTracker = 0;
                }
                else
                {
                    DisableHighlights();
                    options[currentResponseTracker].EnableHighlight();
                }
            }
        }

        void DisableHighlights()
        {
            foreach (var option in options)
            {
                option.DisableHighlight();
            }
        }

        public void InitialSetUp(DialogueNodeBase currentNode, string newName, EventHandler OnDialogueFinished,
            ICommandReceiver commandReceiver)
        {
            currentDialogueType = DialogueType.None;
            this.OnDialogueFinished = OnDialogueFinished;
            this.npcName = newName;
            this.commandReceiver = commandReceiver;
            SetUpDialogue(currentNode);
        }

        

        private void SetUpDialogue(DialogueNodeBase currentNode)
        {
            if (currentNode == null)
            {
                currentNode = fallBackDialogue;
            }

            if (currentNode is LinearDialogue linearDialogue)
            {
                LinearDialogueAsync(linearDialogue);
            }
            else
            {
                SetDialogueText(currentNode.dialogueText);
            }

            SetUpAnswerOptions(currentNode);

            if (currentNode is NPC_CommandDialogue commandDialogue)
            {
                StartCoroutine(ExecuteCommand(commandDialogue));
            }
        }

        private void SetDialogueText(string message, bool isPlayer = false)
        {
            string displayName = isPlayer ? GameConfig.PlayerName : npcName;

            //Update it in a way that when you first start the conversation you get a name, and you use the cashed version, this will save me from typing the name in each dialogue
            //<style=name> is gold predefined font from textMP default style sheet->Default Style Sheet
            dialogueText.text = $"<style=NpcName>{displayName}:</style> {message}";
        }


        private async void LinearDialogueAsync(LinearDialogue linearDialogue)
        {
            currentDialogueType = DialogueType.Linear;
            foreach (var dialogue in linearDialogue.dialogue)
            {
                SetDialogueText(dialogue.text, dialogue.isPlayer);
                taskDelayMilSec = taskDelayMilSecDefault;
                await Task.Delay(taskDelayMilSec);
            }
            SetUpDialogue(linearDialogue.nextNode);
        }

        private void SetUpAnswerOptions(DialogueNodeBase currentNode)
        {
            if (currentNode is NPC_MultipleChoiseDialogue multipleChoiceDialogue)
            {
                currentDialogueType = DialogueType.MultiChoice;
                optionsPanel.gameObject.SetActive(true);
                this.choices = multipleChoiceDialogue.choices;
                for (int i = 0; i < choices.Length; i++)
                {
                    options[i].UpdateText(choices[i].answerText);
                }
                return;
            }

            optionsPanel.gameObject.SetActive(false);
            if (currentNode is SimpleDialogueNode simpleDialogueNode)
            {
                if (simpleDialogueNode.hasExit)
                {
                    EndDialogue();
                    return;
                }
                SetUpDialogue(simpleDialogueNode.nextNode);
            }

        }

        private void EndDialogue()
        {
            currentDialogueType = DialogueType.None;
            OnDialogueFinished?.Invoke(this, EventArgs.Empty);
            //disable the panel after the exit node
            Invoke(nameof(DisablePanel), 1.5f);
        }

        private IEnumerator ExecuteCommand(NPC_CommandDialogue node)
        {
            if (node.delayBeforeEvent > 0)
                yield return new WaitForSeconds(node.delayBeforeEvent);

            //execute the command on the command receiver if its set(should happen on init set up)
            commandReceiver?.ExecuteCommand(node.commandID, node.desiredItem);

            // If there's a next node, move to it automatically
            if (node.nextNode != null)
            {
                SetUpDialogue(node.nextNode);
            }
            /*else
            {
            //run exit if this is the last one
                StartExitSequence();
            }*/
        }
        void DisablePanel()
        {
            gameObject.SetActive(false);
        }
    }
}