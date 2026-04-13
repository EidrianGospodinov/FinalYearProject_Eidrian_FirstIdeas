using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Units.Player
{
    public class PlayerRegen : MonoBehaviour
    {
        private PlayerHealth playerHealth;
        [SerializeField] private Stats playerStats;
        private float regen;
        private void Awake()
        {
            playerHealth = GetComponentInChildren<PlayerHealth>();
            playerHealth.ResetHealth();
            
        }

        private void Start()
        {
            playerStats.upgradeApplied += UpgradeApplied;
            playerHealth.maxHealth = playerStats.GetStat(Stat.Health);
            regen = playerStats.GetStat(Stat.Regeneration);
        }
        private void UpgradeApplied(Stats stats, StatsUpgrade upgrade)
        {
            playerHealth.maxHealth = playerStats.GetStat(Stat.Health);
            regen = playerStats.GetStat(Stat.Regeneration);
            playerHealth.UpdateGraphics();
            print($"upgrade applied. New player health for {gameObject.name} = {playerHealth.maxHealth}");
        }


        private void Update()
        {
            if (playerHealth.gameObject.activeSelf)
            {
                return;
            }
            if (playerHealth.canRegenerate)
            {
                playerHealth.Regen(regen);
            }
        }
    }
}