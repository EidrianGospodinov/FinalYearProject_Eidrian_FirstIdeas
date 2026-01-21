using System;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.Units
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField]Health health;
        [SerializeField] private int damage = 20;
        AiAgent aiAgent;

        private void Start()
        {
            aiAgent = GetComponentInParent<AiAgent>();
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (aiAgent == null || !aiAgent.IsPerformingAttackVisuals || aiAgent.AttackHasLanded)
            {
                return;
            }
            if (other.CompareTag("Player"))
            {
                Debug.Log($"HitBox: {name} hit player");
                aiAgent.AttackHasLanded = true;
                other.gameObject.GetComponentInChildren<Health>().TakeDamage(damage); 
            }
        }
    }
}