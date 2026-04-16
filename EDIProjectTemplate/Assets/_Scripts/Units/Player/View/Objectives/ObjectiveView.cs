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

        public void UnBind()
        {
            objective.OnChanged -= Refresh;
            objective.OnCompleted -= MarkComplete;
        }

        private void Refresh(int currentCount, int targetCount)
        {
            if (targetCount > 0)
            {
                nameText.text = $"{objective.ObjectiveName}: {currentCount}/{targetCount} ";
            }
            else
            {
                nameText.text = objective.ObjectiveName;
            }
        }

        

        private void MarkComplete()
        {
            //with <s/> it doesnt seem to work anymore
            //nameText.text = $"<s>{nameText.text}</s>";
            nameText.fontStyle = FontStyles.Strikethrough;
            Debug.Log("MarkComplete");
        }
    }
}