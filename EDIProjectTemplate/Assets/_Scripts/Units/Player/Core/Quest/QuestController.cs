using System;
using UnityEngine;
using Zenject;

namespace _Scripts.Units.Player.Core
{
    public class QuestController : MonoBehaviour
    {
        [Inject] private QuestManager questManager;
        private EventBinding<OnQuestCompleted> onQuestCompleted;

        [SerializeField] private CampsManager campsManager;
        [SerializeField] private QuestScriptable goToCamp;
        [SerializeField] private QuestScriptable firstMainQuest;

        private void Start()
        {
            onQuestCompleted = EventBus<OnQuestCompleted>.Register(OnQuestCompletedEvent);
        }

        private void OnQuestCompletedEvent(OnQuestCompleted onQuest)
        {
            Debug.Log($"On Quest compleate. my type {firstMainQuest.GetUniqueId()}. other type {onQuest.UniqueId}");

            if (onQuest.UniqueId == firstMainQuest.GetUniqueId())
            {
                Debug.Log("First main quest completed");
                var camp = campsManager.GetCampByName("By the bridge camp");
                if (camp != null)
                {
                    camp.MakeCampAccessible();
                    questManager.SetQuest(new Quest(goToCamp.objectives));
                }
            }
        }   
    }
}