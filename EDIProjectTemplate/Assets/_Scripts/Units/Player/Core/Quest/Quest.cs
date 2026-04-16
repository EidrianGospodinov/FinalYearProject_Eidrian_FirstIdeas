using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    public class Quest 
    {
        public List<Objective> objectives;
        public string UniqueId;
        
        public event System.Action <string>OnQuestCompleted;
        public bool IsComplete => objectives.All(x => x.IsComplete);

        public Quest(List<Objective> objectives)
        {
            this.objectives = objectives;
        }
        
        public void Start()
        {
            foreach (var obj in objectives)
            {
                UniqueId += obj.GetInstanceID();
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
                OnQuestCompleted?.Invoke(UniqueId);
                Stop();
            }
        }

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