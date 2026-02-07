using System;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// used for all shared data between both heroes
/// </summary>
public class HeroCombinedScript : MonoBehaviour
{
    //add this to some attack data
    private float powerUpXpRequired;
    public float currentPowerUpXp;
    public bool CanPowerUp;
    private EventBinding<OnEnemyHit> onEnemyHit;
    private EventBinding<OnUltimate> onUltimate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(float powerUpXpRequired)
    {
        this.powerUpXpRequired = powerUpXpRequired;
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
        currentPowerUpXp += obj.PowerUpXp;
        if (currentPowerUpXp >= powerUpXpRequired)
        {
            //enable the powerUp
            CanPowerUp = true;
            EventBus<GetUltimateEvent>.Trigger(new GetUltimateEvent());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        EventBus<OnEnemyHit>.Unregister(onEnemyHit);
    }
}
