using UnityEngine;

namespace _Scripts.Dialogue.ScriptableDialogue
{
    [CreateAssetMenu(fileName = "NPC Dialogues", menuName = "NPC/Dialogue/Multiple Choice NPC Dialogue")]
    public class NPC_MultipleChoiseDialogue : DialogueNodeBase
    {
        public Choice[] choices = new Choice[3];
    }

    [System.Serializable]
    public struct Choice
    {
        public string answerText;

        [Header("Leave empty to end conversation")]
        public DialogueNodeBase nextNode;
        
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public bool isPlayer;
        [TextArea(3, 10)] public string text;
    }

    public abstract class DialogueNodeBase : ScriptableObject
    {
        [TextArea(3, 10)]
        public string dialogueText;
    }
}