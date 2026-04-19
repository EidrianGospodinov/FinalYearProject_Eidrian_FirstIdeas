using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.Units.Enemy.StatusEffect
{
    [CreateAssetMenu(menuName = "Status Effects/Freeze Slow Effect")]
    public class IceSlowStatusEffectSO : StatusEffectSO
    {
        [Range(0,1)]
        public float slowMovement;
        private AiAgent aiAgent;


        protected override void UpdateEffect(GameObject target)
        {
            if(aiAgent == null)
            {
                Debug.Log("Set up failed, AiAgent component not found");
                return;
            }
            if (isEffectActive)
            {
                aiAgent.SetUpMovementMultiplier(slowMovement);
            }

        }
        protected override void SetTargetData(GameObject target)
        {
            target.TryGetComponent<AiAgent>(out aiAgent);
        }
        

        public override void RemoveEffect(GameObject target)
        {
            base.RemoveEffect(target);   
            aiAgent.SetUpMovementMultiplier(1);
            
        }
    }

}