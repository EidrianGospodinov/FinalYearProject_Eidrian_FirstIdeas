using _Scripts.Units.Player;
using Unity.Cinemachine;
using Unity.Mathematics;

namespace _Scripts.Units.Enemy
{
    
    using UnityEngine;
    public enum StatusEffectType {None,Fire, Ice}

    public abstract class StatusEffectSO : ScriptableObject
    {
        public StatusEffectType EffectType;
        public float activationThreshold;
        public float thresholdReductionMultiplier = 1f;
        public float thresholdReductionEverySecond = 1f;
        public float activeDuration;
        
        public GameObject visualEffectPrefab;
        
        
        public float tickInterval = 0.5f;
        private float tickIntervalCd;
        protected float currentThreshold;
        protected float remainingDuration;

        [HideInInspector] public bool isEffectActive;
        protected bool isBuildupOnlyShow;
        protected GameObject vfxPlaying;

        protected Health health;
        protected AiAgent aiAgent;
        

        public virtual void AddBuildup(float buildupAmount, GameObject target)
        {
            isBuildupOnlyShow = true;
            currentThreshold += buildupAmount;

            if (currentThreshold >= activationThreshold)
            {
                ApplyEffect(target);
            }
        }
        

        protected virtual void ApplyEffect(GameObject target)
        {
            isEffectActive = true;
            remainingDuration = activeDuration;
            SetTargetData(target);
            
            if (visualEffectPrefab != null)
            {
                vfxPlaying = Instantiate(visualEffectPrefab, target.transform.position, Quaternion.identity,
                    target.transform);
            }
        }

        private void SetTargetData(GameObject target)
        {
            health = target.GetComponent<Health>();
            aiAgent = target.GetComponent<AiAgent>();
        }

        public void UpdateCall(GameObject target, float tickAmount)
        {
            if (isEffectActive)
            {
                isBuildupOnlyShow = false;
                remainingDuration -= tickAmount;
                if (remainingDuration <= 0)
                {
                    isEffectActive = false;
                }
            }
            else
            {
                currentThreshold -= (tickAmount * thresholdReductionEverySecond) * thresholdReductionMultiplier;
                if (currentThreshold <=0)
                {
                    isBuildupOnlyShow = false;
                }
            }

            tickIntervalCd += tickAmount;
            if (tickIntervalCd >= tickInterval)
            {
                UpdateEffect(target);
                tickIntervalCd = 0;
            }
        }

        public virtual void UpdateEffect(GameObject target)
        {
            
        }

        public virtual void RemoveEffect(GameObject gameObject)
        {
            isEffectActive = false;
            currentThreshold = 0;
            remainingDuration = 0;
            if (vfxPlaying)
            {
                Destroy(vfxPlaying);
            }
        }

        public virtual bool CanStatusVisualBeRemoved()
        {
            return !(isEffectActive || isBuildupOnlyShow);
        }

        public float GetCurrentThresholdNormalized()
        {
            return currentThreshold / activationThreshold;
        }
        public float GetCurrentDurationThresholdNormalized()
        {
            return remainingDuration / activeDuration;
        }
    }
}