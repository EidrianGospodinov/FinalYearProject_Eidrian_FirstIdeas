using System;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player;
using UnityEngine;

namespace _Scripts.Units
{
    public class HitBox : MonoBehaviour
    {
        private Health health;
        [SerializeField] private int damage = 20;
        AiAgent aiAgent;

        private void Start()
        {
            aiAgent = GetComponentInParent<AiAgent>();
        }

        private void OnTriggerStay(Collider other)
        {
            
            if (aiAgent == null || !aiAgent.IsPerformingAttackVisuals || aiAgent.AttackHasLanded)
            {
                //Debug.Log($"Hit on trigger enter, is performing attack visuals: {aiAgent.IsPerformingAttackVisuals} \n attack has landed: {aiAgent.AttackHasLanded}");
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