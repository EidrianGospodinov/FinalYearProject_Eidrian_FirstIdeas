using System;
using _Scripts.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicDialogueManager : DialogueManager
{
    [Header("Basic Dialogue")]
    [SerializeField] NPC_BasicDialogue Npc;
    [SerializeField] TextMeshProUGUI npcName;
    [SerializeField] TextMeshProUGUI npcDialogueBox;
    [SerializeField] TextMeshProUGUI playerResponse;
    
    private int currentResponseTracker;

    
    protected override void StartConversation()
    {
        base.StartConversation();
        currentResponseTracker = 0;
        npcName.text = Npc.name;
        npcDialogueBox.text = Npc.dialogue[0];
    }
}
