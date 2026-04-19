using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.Units.Enemy.StatusEffect
{
    [CreateAssetMenu(menuName = "Status Effects/Freeze Slow Effect")]
    public class IceSlowStatusEffectSO : StatusEffectSO
    {
        [Range(0,1)]
        public float slowMovement;

        public override void UpdateEffect(GameObject target)
        {
            base.UpdateEffect(target);
            if (isEffectActive)
            {
                aiAgent.SetUpMovementMultiplier(slowMovement);
            }

        }

        public override void RemoveEffect(GameObject target)
        {
            base.RemoveEffect(target);   
            aiAgent.SetUpMovementMultiplier(1);
            
        }
    }

}