public class TestEvent : IEvent { }

public class PlayerEvent : IEvent
{
    public int PlayerID { get; set; }
}