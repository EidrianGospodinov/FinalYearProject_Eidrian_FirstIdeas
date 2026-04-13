using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Units.Player
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        
        public float currentHealth { get; set; }
        private bool isDead;

        private void Start()
        {
            currentHealth = maxHealth;
            OnStart();
        }
        

        public void TakeDamage(float amount)
        {
            if (isDead)
            {
                return;
            }
            Debug.Log($"Health prev: {currentHealth}/{maxHealth}");

            if (!IsDead())
            {
                currentHealth -= amount;
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