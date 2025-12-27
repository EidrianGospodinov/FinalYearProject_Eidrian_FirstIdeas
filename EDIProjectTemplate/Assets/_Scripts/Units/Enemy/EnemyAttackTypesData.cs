using UnityEngine;

namespace _Scripts.Units.Enemy
{
    [CreateAssetMenu(fileName = "AttackType", menuName = "Game/Enemy/AttackType")]
    public class EnemyAttackTypesData : ScriptableObject
    {
        public string attackName;     
        public int weight;            
        public float minDistance;     
        public float maxDistance;     
    }
}