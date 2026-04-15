using System;
using _Scripts.Units.Player.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Units.Player.View
{
    public class ObjectivesManagerView : MonoBehaviour
    {
        [Inject] private QuestManager questManager;
        [SerializeField] private ObjectiveView objectiveViewPrefab;
        [SerializeField] private VerticalLayoutGroup verticalLayoutGroup; 

        private void Start()
        {
            questManager.OnQuestPopulate += OnQuestPopulated;
        }

        private void OnQuestPopulated()
        {
            var currentQuest = questManager.GetCurrentQuest();
            var objectivesList = currentQuest.GetObjectivesList();
            

            foreach(var objective in objectivesList)
            {
                var objectInstance = Instantiate(objectiveViewPrefab, verticalLayoutGroup.transform);
                objectInstance.Bind(objective);
            }
        }
    }
}