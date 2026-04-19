using System;
using _Scripts.Units.Enemy;
using _Scripts.Units.Enemy.StatusEffect;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Units.Player
{
    [Serializable]
    public class DamageData
    {
        public float damage;
        public StatusEffectType effectType;
        public float buildupAmount;

        public DamageData(float damage, StatusEffectType effectType, float buildupAmount)
        {
            this.damage = damage;
            this.effectType = effectType;
            this.buildupAmount = buildupAmount;
        }
    }
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        
        public float currentHealth { get; set; }
        protected bool isDead;
        private StatusEffectManager statusEffectManager;

        private void Start()
        {
            currentHealth = maxHealth;
            OnStart();
            TryGetComponent<StatusEffectManager>(out statusEffectManager);
        }
        

        public void TakeDamage(DamageData damageData)
        {
            if (isDead)
            {
                return;
            }
            Debug.Log($"Health prev: {currentHealth}/{maxHealth}");

            if (!IsDead())
            {
                if (statusEffectManager != null && damageData.effectType != StatusEffectType.None)
                {
                    statusEffectManager.OnStatusTriggerBuildup(damageData.effectType, damageData.buildupAmount);
                }
                currentHealth -= damageData.damage;
                Debug.Log($"Health: {currentHealth}/{maxHealth}");
                OnDamage();
            }
            if(IsDead())
            {
                isDead = true;
                OnDeath();
            }
        }
        public bool IsDead()
        {
            return currentHealth <= 0.0f;
        }
        protected virtual void OnStart()
        {

        }
        protected virtual void OnDeath()
        {
        }
        protected virtual void OnDamage()
        {

        }
        
    }
}