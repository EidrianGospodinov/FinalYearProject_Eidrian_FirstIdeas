using System.Collections.Generic;
using System.Linq;
using _Scripts.Units.Enemy;

namespace _Scripts.Units.Player.Core
{
    public abstract class Objective
    {
        public bool IsComplete { get; protected set; }
        public event System.Action OnCompleted;

        protected void Complete()
        {
            if(IsComplete) return;
            IsComplete = true;
            OnCompleted?.Invoke();
            
        }
        public abstract void Initialize();
        public abstract void Dispose();
    }
    public class KillEnemyObjective : Objective
    {
        private EventBinding<OnEnemyRemoved> enemyRemoved;
        private int currentCount = 0;
        private int targetCount = 0;

        public KillEnemyObjective(int targetCount)
        {
            this.targetCount = targetCount;
        }
        
        public override void Initialize()
        {
            enemyRemoved = EventBus<OnEnemyRemoved>.Register(OnEnemyRemoved);
        }

        public override void Dispose()
        {
            EventBus<OnEnemyRemoved>.Unregister(enemyRemoved);
        }

        private void OnEnemyRemoved(OnEnemyRemoved e)
        {
            currentCount++;

            if (currentCount >= targetCount)
                Complete();
        }
    }

    public class Quest
    {
        private List<Objective> objectives;
        
        public event System.Action OnQuestCompleted;
        public bool IsComplete => objectives.All(x => x.IsComplete);

        public Quest(List<Objective> objectives)
        {
            this.objectives = objectives;
        }
        
        public void Start()
        {
            foreach (var obj in objectives)
            {
                obj.Initialize();
                obj.OnCompleted += CheckQuest;
            }
        }

        private void CheckQuest()
        {
            if (IsComplete)
            {
                Stop();
                OnQuestCompleted?.Invoke();
            }        }

        public void Stop()
        {
            foreach (var obj in objectives)
            {
                obj.OnCompleted -= CheckQuest;
                obj.Dispose();
            }
        }        
    }

    public class QuestManager
    {
        private Quest currentQuest;
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
                currentQuest.Start();
            }
            
        }
        
        private void HandleQuestCompleted()
        {
            EventBus<OnQuestCompleted>.Trigger(new OnQuestCompleted());
        }
        
    }

    public class OnQuestCompleted : IEvent
    {
    }
}