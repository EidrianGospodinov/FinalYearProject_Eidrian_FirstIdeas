using _Scripts.Units.Enemy;
using _Scripts.Units.Player.Combat;
using UnityEngine;

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

        public virtual void Dispose()
        {
            OnCompleted = null;
            OnChanged = null;
        }
        
    }

    public class FindObjectObjective : Objective
    {
        private EventBinding<OnItemFound> itemFound;
        public override void Initialize()
        {
            itemFound = EventBus<OnItemFound>.Register(OnItemFoundEvent);
        }

        public FindObjectObjective(string objectiveName)
        {
            ObjectiveName = objectiveName;
        }
        private void OnItemFoundEvent(OnItemFound obj)
        {
            Complete();
        }

        public override void Dispose()
        {
            base.Dispose();
            EventBus<OnItemFound>.Unregister(itemFound);
        }
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
            base.Dispose();
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