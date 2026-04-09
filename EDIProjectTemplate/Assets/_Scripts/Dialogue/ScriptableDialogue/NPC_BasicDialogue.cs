using UnityEngine;

[CreateAssetMenu(fileName = "NPC Dialogues", menuName = "Basic NPC Dialogue")]
public class NPC_BasicDialogue : ScriptableObject
{
    [TextArea(3, 15)] public string[] dialogue;
    [TextArea(3, 15)] public string[] playerDialogue;

}