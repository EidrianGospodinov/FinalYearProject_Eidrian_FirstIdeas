using System;
using _Scripts.Units.Player;
using UnityEngine;

public class VFXCollisionDetection : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Stat stat;
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
            other.gameObject.GetComponent<Health>().TakeDamage(damage); 
            
        }
    }
}
