using _Scripts.Units.Enemy;
using _Scripts.Units.Player;
using UnityEngine;

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

    public OnUltimate(HeroData heroData, Transform target)
    {
        HeroData = heroData;
        this.target = target;
    }

}
public class OnLongRange : IEvent
{
    public HeroData HeroData;
    public Transform target;

    public OnLongRange(HeroData heroData, Transform target)
    {
        HeroData = heroData;
        this.target = target;
        var enemyHeight = this.target.GetComponent<AiAgent>().agentConfig.Height;
        //setting up the height of the attack effect
        var newPos = this.target.position;
        newPos.y = enemyHeight;
        this.target.position = newPos;

    }

}

public class GetUltimateEvent : IEvent
{
    
}

public class OnEnemyHit : IEvent
{
    public float PowerUpXp;
    public OnEnemyHit(float powerUpXp)
    {
        PowerUpXp = powerUpXp;
    }
}