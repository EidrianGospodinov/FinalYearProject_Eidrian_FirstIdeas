using _Scripts.Units.Player;

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