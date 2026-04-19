using System;
using System.Collections.Generic;
using System.Linq;
using GDX.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Units.Enemy.StatusEffect
{
    public class StatusEffectManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<StatusEffectType, StatusEffectSO> statusEffectToApplyDict = new();
        private SerializableDictionary<StatusEffectType, StatusEffectSO> enabledEffects = new();

        private Dictionary<StatusEffectType, StatusEffectSO> statusEffectCashedDict = new();

        [SerializeField, Tooltip("Run the update call in statusEffect so every what interval")]
        private float interval = 0.1f;
        private float currentInterval = 0f;
        private float lastInterval = 0f;


        public UnityAction<StatusEffectSO, float> ActiveStatus;
        public UnityAction<StatusEffectSO> DeactivateStatusEffect;
        public UnityAction<StatusEffectSO, float,float> UpdateStatusEffect;
        
        private void Start()
        {
        }

        public void OnStatusTriggerBuildup(StatusEffectType effectType, float amount)
        {
            if (!enabledEffects.ContainsKey(effectType))
            {
                var effectToAdd = CreateEffectObject(effectType, statusEffectToApplyDict[effectType]);
                enabledEffects[effectType] = effectToAdd;
                
                ActiveStatus?.Invoke(effectToAdd, effectToAdd.GetCurrentDurationThresholdNormalized());
                
            }

            var statusEffectSo = enabledEffects[effectType];
            if (!statusEffectSo.isEffectActive)
            {
                statusEffectSo.AddBuildup(amount, gameObject);
                
                UpdateStatusEffect?.Invoke(statusEffectSo, statusEffectSo.GetCurrentThresholdNormalized(), statusEffectSo.GetCurrentDurationThresholdNormalized());
            }
            else
            {
                int tickDamageAmount = (int)Mathf.Ceil(amount / 4);
                //call health damage func 
            }
        }

        private StatusEffectSO CreateEffectObject(StatusEffectType statusEffectType, StatusEffectSO effectSo)
        {
            if (!statusEffectCashedDict.ContainsKey(statusEffectType))
            {
                statusEffectCashedDict[statusEffectType] = Instantiate(effectSo);
            }

            return statusEffectCashedDict[statusEffectType];
        }

        public void UpdateEffect(GameObject target)
        {
            foreach (var effect in enabledEffects.ToList())
            {
                effect.Value.UpdateCall(target, interval);

                var effectValue = effect.Value;
                UpdateStatusEffect?.Invoke(effectValue, effectValue.GetCurrentThresholdNormalized(), effectValue.GetCurrentDurationThresholdNormalized() );
                
                if (effect.Value.CanStatusVisualBeRemoved())
                {
                    RemoveEffect(effect.Key);
                }
            }
            
        }

        public void RemoveEffect(StatusEffectType effectType)
        {
            if (enabledEffects.ContainsKey(effectType))
            {
                enabledEffects[effectType].RemoveEffect(gameObject);
                
                DeactivateStatusEffect?.Invoke(enabledEffects[effectType]);
                enabledEffects.Remove(effectType);
            }
        }

        void Update()
        {
            currentInterval += Time.deltaTime;
            if (currentInterval > lastInterval + interval)
            {
                UpdateEffect(gameObject);
                lastInterval = currentInterval;
            }
            /*foreach (var effect in activeEffects.Values)
            {
                effect.UpdateEffect(gameObject);
            }*/
        }
    }
}