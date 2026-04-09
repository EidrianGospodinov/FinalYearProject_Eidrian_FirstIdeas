using _Scripts.Dialogue.ScriptableDialogue;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Dialogues", menuName = "NPC/Dialogue/Command Dialogue")]
public class NPC_CommandDialogue : DialogueNodeBase
{
    [Header("Command Settings")]
    public string commandID;

    public Item desiredItem;
    public float delayBeforeEvent;
    
    [Header("Navigation")]
    public DialogueNodeBase nextNode;
}