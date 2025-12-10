using _Scripts.StateMachine.PlayerActionStateMachine;

public class OnAttack : IEvent
{
    public OnAttack(AttackType attackType, ComboStateId sequenceID = ComboStateId.WindDown)
    {
        this.AttackType = attackType;
        ComboStateId = sequenceID;
    }
    public readonly AttackType AttackType;
    public ComboStateId ComboStateId { get; set; }
}

public enum AttackType
{
     NONE = 0,
     Sword,
     Special
};