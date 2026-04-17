using UnityEditor.Search;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    [System.Serializable]
    public abstract class Objective : ScriptableObject
    {
        public string ObjectiveName;
        public bool IsComplete { get; protected set; }
        
        protected int currentCount = 0;
        public int targetCount = 0;
        public event System.Action OnCompleted;
        public event System.Action<int, int> OnChanged;

        protected void Complete()
        {
            if(IsComplete) return;
            IsComplete = true;
            OnChanged?.Invoke(currentCount, targetCount);
            OnCompleted?.Invoke();
            
        }
        protected void NotifyChanged()
        {
            OnChanged?.Invoke(currentCount, targetCount);
        }

        public virtual void Initialize()
        {
            currentCount = 0;
            IsComplete = false;
        }

        public virtual void Dispose()
        {
            /*OnCompleted = null;
            OnChanged = null;*/
            currentCount = 0;
            IsComplete = false;
        }
        
    }

    public class OnQuestCompleted : IEvent
    {
        public string UniqueId;
        public OnQuestCompleted(string uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}