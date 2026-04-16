namespace _Scripts.Units.Player.Core
{
    public class QuestManager
    {
        private Quest currentQuest;
        public event System.Action <Quest>OnQuestPopulate;
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
                OnQuestPopulate?.Invoke(currentQuest);
                currentQuest.Start();
            }
            
        }
        
        
        private void HandleQuestCompleted(string uniqueId)
        {
            EventBus<OnQuestCompleted>.Trigger(new OnQuestCompleted(uniqueId));
        }
        
    }
}