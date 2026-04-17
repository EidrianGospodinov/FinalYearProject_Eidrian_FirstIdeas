using System.Collections.Generic;
using System.Linq;
using _Scripts.New_Folder.Spawner;
using UnityEngine;

namespace _Scripts.Units.Enemy
{
    public class EnemyManager
    {
        private readonly List<AiAgent> enemies = new List<AiAgent>();

        public void RegisterEnemy(AiAgent aiAgent)
        {
            if (!enemies.Contains(aiAgent))
                enemies.Add(aiAgent);
        }
        

        public void UnregisterEnemy(AiAgent aiAgent, bool destroy = false)
        {
            //decreases the number of enemies from that spawner
            aiAgent.EnemySpawner?.NotifyDeath(destroy);
            enemies.Remove(aiAgent);
            if (!destroy)
            {
                EventBus<OnEnemyRemoved>.Trigger(new OnEnemyRemoved(aiAgent));
            }
        }

        public void DestroyAllEnemiesFromArea(EnemySpawner enemySpawner)
        {
            bool destroy = true;
            //Bug fix- InvalidOperationException
            //looping backwards to remove item without breaking the index
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];
                if (enemy.EnemySpawner == enemySpawner)
                {
                    UnregisterEnemy(enemy, destroy);
                    Object.Destroy(enemy.gameObject);
                }
            }
        }

        public void SetEnemiesInSafeZone(bool value)
        {
            foreach (var enemy in enemies)
            {
                enemy.SetPlayerInSafeZone(value);
            }
        }

        public IEnumerable<AiAgent> GetAllEnemies() => enemies;
    }

    public class OnEnemyRemoved : IEvent
    {
        public AiAgent AiAgent { get; private set; }
        public OnEnemyRemoved(AiAgent aiAgent)
        {
            AiAgent = aiAgent;
        }
    }
}