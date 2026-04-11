using _Scripts.StateMachine.PlayerActionStateMachine;

public class OnAttack : IEvent
{
    public OnAttack(AttackType attackType, ComboStateId sequenceID = ComboStateId.WindDown, HeroType heroType = HeroType.Oreon)
    {
        this.AttackType = attackType;
        ComboStateId = sequenceID;
        HeroType = heroType;
    }
    public readonly AttackType AttackType;
    public ComboStateId ComboStateId { get; set; }
    public HeroType HeroType { get; set; }
}

public enum AttackType
{
     NONE = 0,
     Sword,
     Special,
     LongRange
};