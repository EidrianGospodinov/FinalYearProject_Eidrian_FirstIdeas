using UnityEngine;

namespace _Scripts.Dialogue.ScriptableDialogue
{
    [CreateAssetMenu(fileName = "NPC Dialogues", menuName = "NPC/Dialogue/Single Line Dialogue")]
    public class SimpleDialogueNode : DialogueNodeBase
    {
        [Header("This Dialogue type is used for dialogue exit")]
        public bool hasExit = false;
        public DialogueNodeBase nextNode;
    }
}