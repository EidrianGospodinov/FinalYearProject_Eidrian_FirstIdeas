using System;
using _Scripts.Units.Enemy;
using UnityEngine;
using Zenject;

namespace _Scripts.Units.Player.Core
{
    public class QuestController : MonoBehaviour
    {
        [Inject] private QuestManager questManager;
        [Inject] private EnemyManager enemyManager;
        private EventBinding<OnQuestCompleted> onQuestCompleted;

        [SerializeField] private CampsManager campsManager;
        [SerializeField] private QuestScriptable goToCamp;
        [SerializeField] private QuestScriptable firstMainQuest;
        [SerializeField] private QuestScriptable minotaurQuest;
        [SerializeField] private QuestScriptable finalQuest;

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
                UnlockAndSetUpNextCamp("By the bridge camp");
            }
            else if (onQuest.UniqueId == minotaurQuest.GetUniqueId())
            {
                UnlockAndSetUpNextCamp("The ruins");   
            }
            else if (onQuest.UniqueId == finalQuest.GetUniqueId())
            {
                enemyManager.FinalQuestCompleted = true;//todo: make this fire an event called all quest completed instead of injecting the enemy manager where its not needed
                UnlockAndSetUpNextCamp("The cathedral");   
            }
        }

        private void UnlockAndSetUpNextCamp(string nameOfCamp)
        {
            var camp = campsManager.GetCampByName(nameOfCamp);
            if (camp != null)
            {
                camp.MakeCampAccessible();
                questManager.SetQuest(new Quest(goToCamp.objectives));
            }
        }
    }
}