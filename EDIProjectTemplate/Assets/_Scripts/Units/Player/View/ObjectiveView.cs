using _Scripts.Units.Player.Core;
using TMPro;
using UnityEngine;

namespace _Scripts.Units.Player.View
{
    public class ObjectiveView : MonoBehaviour
    {
        private Objective objective;
        [SerializeField] private TextMeshProUGUI nameText;
        

        public void Bind(Objective obj)
        {
            objective = obj;

            objective.OnChanged += Refresh;
            objective.OnCompleted += MarkComplete;
            Refresh(0, objective.targetCount);

        }

        private void Refresh(int currentCount, int targetCount)
        {
            nameText.text = $"{objective.ObjectiveName}: {currentCount}/{targetCount} ";
        }

        

        private void MarkComplete()
        {
            nameText.text = $"<s>{nameText.text}</s>";
        }
    }
}