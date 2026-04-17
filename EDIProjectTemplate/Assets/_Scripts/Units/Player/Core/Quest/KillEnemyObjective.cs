using _Scripts.Units.Enemy;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    [CreateAssetMenu(menuName = "Objective/Kill Enemy")]
    public class KillEnemyObjective : Objective
    {
        private EventBinding<OnEnemyRemoved> enemyRemoved;
        public EnemyType EnemyType;
        

        /*public KillEnemyObjective(string objectiveName, int targetCount)
        {
            this.targetCount = targetCount;
            ObjectiveName = objectiveName;
        }*/
        
        public override void Initialize()
        {
            base.Initialize();
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
            if (EnemyType == EnemyType.Any || e.AiAgent.agentConfig.EnemyType == EnemyType)
            {
                currentCount++;
                NotifyChanged();
                if (currentCount >= targetCount)
                {
                    Complete();
                }
            }
        }
    }
    
}