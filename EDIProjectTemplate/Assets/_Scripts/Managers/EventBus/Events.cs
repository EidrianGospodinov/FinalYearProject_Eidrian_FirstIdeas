using _Scripts.Units.Enemy;
using _Scripts.Units.Player;
using _Scripts.Units.Player.Core;
using UnityEngine;
using Zenject;

public class TestEvent : IEvent { }

public class PlayerEvent : IEvent
{
    public int PlayerID { get; set; }
}

public class OnSwitchHeroEvent : IEvent
{
    public HeroData HeroData;

    public OnSwitchHeroEvent(HeroData heroData)
    {
        HeroData = heroData;
    }
}
public class OnUltimate : IEvent
{
    public HeroData HeroData;
    public Transform target;

    public OnUltimate(HeroData heroData, Transform target, PlayerServices playerServices)
    {
        playerServices.HasUltimate = false;
        HeroData = heroData;
        this.target = target;
    }

}
public class OnLongRange : IEvent
{
    public HeroData HeroData;
    public readonly Transform EnemyTarget;

    public OnLongRange(HeroData heroData, Transform target)
    {
        HeroData = heroData;
        EnemyTarget = target.GetComponent<AiAgent>().LongRangeTarget;
    }

}

public class GetUltimateEvent : IEvent
{
    public GetUltimateEvent(PlayerServices playerServices)
    {
        playerServices.HasUltimate = true;
    }
}

public class OnEnemyHit : IEvent
{
    public float PowerUpXp;
    public Health EnemyHealth;
    public OnEnemyHit(float powerUpXp, Health enemyHealth)
    {
        PowerUpXp = powerUpXp;
        EnemyHealth = enemyHealth;
    }
}