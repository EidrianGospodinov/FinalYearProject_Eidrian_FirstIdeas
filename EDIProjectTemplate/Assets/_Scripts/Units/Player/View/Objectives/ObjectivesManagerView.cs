using System;
using System.Collections.Generic;
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
        
        private List<ObjectiveView> objectViewInstances = new List<ObjectiveView>();

        private void Start()
        {
            questManager.OnQuestPopulate += OnQuestPopulated;
        }

        private void OnQuestPopulated()
        {
            DeleteObjectives();
            var currentQuest = questManager.GetCurrentQuest();
            var objectivesList = currentQuest.GetObjectivesList();
            

            foreach(var objective in objectivesList)
            {
                var objectInstance = Instantiate(objectiveViewPrefab, verticalLayoutGroup.transform);
                objectViewInstances.Add(objectInstance);
                objectInstance.Bind(objective);
            }
        }

        private void DeleteObjectives()
        {
            if (objectViewInstances.Count <= 0)
            {
                return;
            }
            for (int i = objectViewInstances.Count - 1; i >= 0; i--)
            {
                Destroy(objectViewInstances[i].gameObject);
            }
            objectViewInstances.Clear();
        }
    }
}