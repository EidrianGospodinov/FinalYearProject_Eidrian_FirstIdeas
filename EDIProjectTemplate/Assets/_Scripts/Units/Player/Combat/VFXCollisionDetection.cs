using System;
using _Scripts;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class VFXCollisionDetection : MonoBehaviour
{
    [Inject] private DynamicTextServices dynamicTextServices;
    
    [SerializeField] private Stats stats;
    [SerializeField] private Stat stat;

    [SerializeField] private DamageData damageData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (stats == null)
            {
                Debug.LogError("Stats class is not serialized");
                return;
            }
            var damage = stats.GetStat(stat);
            var agent = other.gameObject.GetComponent<AiAgent>();
            var updatedDamage = dynamicTextServices.HandleDamageVisuals(transform, other, agent, damage);
            
            if (damageData.effectType == StatusEffectType.None)
            {
                return;
            }
            damageData.damage = updatedDamage;
            other.gameObject.GetComponent<Health>().TakeDamage(damageData); 
            
        }
    }
}
