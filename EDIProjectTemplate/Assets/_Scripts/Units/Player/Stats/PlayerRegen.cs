using System;
using UnityEngine;

namespace _Scripts.Units.Player
{
    public class PlayerRegen : MonoBehaviour
    {
        private PlayerHealth playerHealth;
        
        private void Awake()
        {
            playerHealth = GetComponentInChildren<PlayerHealth>();
        }
        

        private void Update()
        {
            if (playerHealth.gameObject.activeSelf)
            {
                return;
            }
            if (playerHealth.Regenerate)
            {
                playerHealth.Regen();
            }
        }
    }
}