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

    public override void OnMouseOver()
    {
        base.OnMouseOver();
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            currentResponseTracker++;
            if (currentResponseTracker >= Npc.playerDialogue.Length - 1)
            {
                currentResponseTracker = Npc.playerDialogue.Length - 1;
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            currentResponseTracker--;
            if (currentResponseTracker < 0)
            {
                currentResponseTracker = 0;
            }
        }

        if (Npc.playerDialogue.Length >= currentResponseTracker)
        {
            playerResponse.text = Npc.playerDialogue[currentResponseTracker];
            if (Input.GetKeyDown(KeyCode.Return))
            {
                npcDialogueBox.text = Npc.dialogue[currentResponseTracker + 1];
            }
        }
    }
    
    

    protected override void StartConversation()
    {
        base.StartConversation();
        currentResponseTracker = 0;
        npcName.text = Npc.name;
        npcDialogueBox.text = Npc.dialogue[0];
    }
}
