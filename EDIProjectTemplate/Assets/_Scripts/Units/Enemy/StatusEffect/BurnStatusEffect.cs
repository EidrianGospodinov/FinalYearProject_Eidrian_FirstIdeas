using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.Units.Enemy
{
    [CreateAssetMenu(menuName = "Status Effects/Burn")]
    public class BurnStatusEffectSO : StatusEffectSO
    {
        public float damagePerTick = 5f;

        public override void UpdateEffect(GameObject target)
        {
            base.UpdateEffect(target);
            if (isEffectActive)
            {
                health.TakeDamage(new DamageData(damagePerTick, EffectType, 0));    
            }
            
        }
    }
}