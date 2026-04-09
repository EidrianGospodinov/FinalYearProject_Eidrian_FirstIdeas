using System;
using _Scripts.Dialogue.MultipleChoiceDialogue;
using _Scripts.Dialogue.NPCTypes;
using _Scripts.Dialogue.ScriptableDialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Dialogue
{
    public class MultipleChoiceDialogueManager : DialogueManager
    {
        [FormerlySerializedAs("Npc")]
        [Header("Multiple Choice Dialogue")]
        [SerializeField] DialogueNodeBase NpcDialogue;

        [SerializeField] private NPC npc;
        private MultipleChoiceDialogueMenu multipleChoiceDialogueMenu;
        
        public event EventHandler OnDialogueFinished;
        private ICommandReceiver commandReceiver;

        protected override void Start()
        {
            base.Start();
            multipleChoiceDialogueMenu = dialogueUI.GetComponent<MultipleChoiceDialogueMenu>();
            OnDialogueFinished += OnDialogueFinishedEvent;
            commandReceiver = GetComponent<ICommandReceiver>();
        }

        private void OnDialogueFinishedEvent(object sender, EventArgs e)
        {
            EndConversation();
        }
        
        protected override void StartConversation()
        {
            base.StartConversation();
            multipleChoiceDialogueMenu.InitialSetUp(NpcDialogue, npc.npcName, OnDialogueFinished, commandReceiver);
        }
    }
}
