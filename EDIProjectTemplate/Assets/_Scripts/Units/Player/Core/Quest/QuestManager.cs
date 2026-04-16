namespace _Scripts.Units.Player.Core
{
    public class QuestManager
    {
        private Quest currentQuest;
        public event System.Action OnQuestPopulate;
        public void SetQuest(Quest newQuest)
        {
            if (currentQuest != null)
            {
                currentQuest.OnQuestCompleted -= HandleQuestCompleted;
                currentQuest?.Stop();
            }

            currentQuest = newQuest;
            if (currentQuest != null)
            {
                currentQuest.OnQuestCompleted += HandleQuestCompleted;
                OnQuestPopulate?.Invoke();
                currentQuest.Start();
            }
            
        }

        public Quest GetCurrentQuest()
        {
            return currentQuest;
        }
        
        private void HandleQuestCompleted()
        {
            EventBus<OnQuestCompleted>.Trigger(new OnQuestCompleted());
        }
        
    }
}