using _Scripts.Units.Enemy;

namespace _Scripts.Units.Player.Core
{
    public abstract class Objective
    {
        public string ObjectiveName { get; protected set; }
        public bool IsComplete { get; protected set; }
        
        protected int currentCount = 0;
        public int targetCount = 0;
        public event System.Action OnCompleted;
        public event System.Action<int, int> OnChanged;

        protected void Complete()
        {
            if(IsComplete) return;
            IsComplete = true;
            OnCompleted?.Invoke();
            OnChanged?.Invoke(currentCount, targetCount);
            
        }
        protected void NotifyChanged()
        {
            OnChanged?.Invoke(currentCount, targetCount);
        }
        public abstract void Initialize();
        public abstract void Dispose();
    }
    public class KillEnemyObjective : Objective
    {
        private EventBinding<OnEnemyRemoved> enemyRemoved;
        

        public KillEnemyObjective(string objectiveName, int targetCount)
        {
            this.targetCount = targetCount;
            ObjectiveName = objectiveName;
        }
        
        public override void Initialize()
        {
            enemyRemoved = EventBus<OnEnemyRemoved>.Register(OnEnemyRemoved);
            NotifyChanged();
        }

        public override void Dispose()
        {
            EventBus<OnEnemyRemoved>.Unregister(enemyRemoved);
        }

        private void OnEnemyRemoved(OnEnemyRemoved e)
        {
            currentCount++;
            NotifyChanged();
            if (currentCount >= targetCount)
                Complete();
        }
    }

    public class OnQuestCompleted : IEvent
    {
    }
}