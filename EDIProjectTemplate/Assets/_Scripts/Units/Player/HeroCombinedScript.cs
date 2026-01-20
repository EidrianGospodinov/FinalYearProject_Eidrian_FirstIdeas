using System;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// used for all shared data between both heroes
/// </summary>
public class HeroCombinedScript : MonoBehaviour
{
    [SerializeField]
    //add this to some attack data
    private float powerUpXpRequired;
    public float currentPowerUpXp;
    public bool CanPowerUp;
    private EventBinding<OnEnemyHit> onEnemyHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(float powerUpXpRequired)
    {
        this.powerUpXpRequired = powerUpXpRequired;
    }
    void OnEnable()
    {
        onEnemyHit = EventBus<OnEnemyHit>.Register(OnEnemyHitEvent);
    }

    private void OnEnemyHitEvent(OnEnemyHit obj)
    {
        currentPowerUpXp += obj.PowerUpXp;
        if (currentPowerUpXp >= powerUpXpRequired)
        {
            //enable the powerUp
            CanPowerUp = true;
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
