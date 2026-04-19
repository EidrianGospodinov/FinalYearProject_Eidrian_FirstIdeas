using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.Units.Enemy.StatusEffect
{
    [CreateAssetMenu(menuName = "Status Effects/Burn")]
    public class BurnStatusEffectSO : StatusEffectSO
    {
        public float damagePerTick = 5f;
        private Health health;
        protected override void UpdateEffect(GameObject target)
        {
            if (health == null)
            {
                Debug.Log("Set up failed, Health component not found");
                return;
            }
            if (isEffectActive)
            {
                health.TakeDamage(new DamageData(damagePerTick, EffectType, 0));    
            }
            
        }

        protected override void SetTargetData(GameObject target)
        {
            target.TryGetComponent<Health>(out health);
        }
    }
}