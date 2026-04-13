using System.Collections.Generic;

namespace _Scripts.Units.Enemy
{
    public class EnemyManager
    {
        private readonly List<AiAgent> _enemies = new List<AiAgent>();

        public void RegisterEnemy(AiAgent aiAgent)
        {
            if (!_enemies.Contains(aiAgent))
                _enemies.Add(aiAgent);
        }

        public void UnregisterEnemy(AiAgent aiAgent)
        {
            //decreases the number of enemies from that spawner
            aiAgent.EnemySpawner?.NotifyDeath();
            _enemies.Remove(aiAgent);
        }

        public void SetEnemiesInSafeZone(bool value)
        {
            foreach (var enemy in _enemies)
            {
                enemy.SetPlayerInSafeZone(value);
            }
        }

        public IEnumerable<AiAgent> GetAllEnemies() => _enemies;
    }
}