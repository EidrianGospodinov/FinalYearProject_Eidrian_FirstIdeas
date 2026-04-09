using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Dialogue.ScriptableDialogue
{
    [CreateAssetMenu(fileName = "NPC Dialogues", menuName = "NPC/Dialogue/Linear Dialogue_NoChoices")]
    public class LinearDialogue : DialogueNodeBase
    {
        public List<DialogueLine> dialogue;
        public DialogueNodeBase nextNode;
    }
}