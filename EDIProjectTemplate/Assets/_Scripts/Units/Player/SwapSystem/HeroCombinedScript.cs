using System;
using _Scripts.Units.Player;
using _Scripts.Units.Player.Core;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

/// <summary>
/// used for all shared data between both heroes
/// </summary>
public class HeroCombinedScript : MonoBehaviour
{
    [Inject] private PlayerServices playerServices;
    //add this to some attack data
    private float powerUpXpRequired;
    public float currentPowerUpXp;
    public bool CanPowerUp;
    private EventBinding<OnEnemyHit> onEnemyHit;
    private EventBinding<OnUltimate> onUltimate;

    private WeaponManager weaponManager;
    private Stats playerStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(Stats playerStats)
    {
        this.playerStats = playerStats;
        powerUpXpRequired = playerStats.GetStat(Stat.PowerUpXpRequired);
    }

    public void InitSwordFound(WeaponManager weaponManager)
    {
        this.weaponManager = weaponManager;
    }

    public void UpdatePowerUpXpRequired()
    {
        powerUpXpRequired = playerStats.GetStat(Stat.PowerUpXpRequired);
    }
    void OnEnable()
    {
        onEnemyHit = EventBus<OnEnemyHit>.Register(OnEnemyHitEvent);
        onUltimate = EventBus<OnUltimate>.Register(OnUltimateEvent);

    }

    private void OnUltimateEvent(OnUltimate obj)
    {
        CanPowerUp = false;
        currentPowerUpXp = 0;
    }

    private void OnEnemyHitEvent(OnEnemyHit obj)
    {
        UpdateSwordIntensity();
        
        if (obj.EnemyHealth.IsDead())
        {
            return;
        }
        currentPowerUpXp += obj.PowerUpXp;
        if (currentPowerUpXp >= powerUpXpRequired)
        {
            //enable the powerUp
            CanPowerUp = true;
            EventBus<GetUltimateEvent>.Trigger(new GetUltimateEvent(playerServices));
        }
    }

    public void UpdateSwordIntensity()
    {
        if (weaponManager != null)
        {
            float normalizedXp = Mathf.Clamp01(currentPowerUpXp / powerUpXpRequired);
            float intensity = normalizedXp * 7;
            weaponManager.UpdateSwordIntensity(intensity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        EventBus<OnEnemyHit>.Unregister(onEnemyHit);
        EventBus<OnUltimate>.Unregister(onUltimate);
    }
}
