using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Units.Player
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public Slider healthBar;
        public float currentHealth { get; set; }

        private void Start()
        {
            currentHealth = maxHealth;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
            OnStart();
        }
        

        public void TakeDamage(float amount)
        {
            Debug.Log($"Health prev: {currentHealth}/{maxHealth}");
            currentHealth -= amount;
            Debug.Log($"Health: {currentHealth}/{maxHealth}");
            
            healthBar.value = currentHealth;
            OnDamage();
            if (currentHealth <= 0.0f)
            {
                OnDeath();
            }
        }
        public bool IsDead()
        {
            return currentHealth <= 0;
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