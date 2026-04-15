using System.Collections.Generic;
using System.Linq;

namespace _Scripts.Units.Player.Core
{
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

        public List<Objective> GetObjectivesList()
        {
            return objectives;
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
}