public class OnAttack : IEvent
{
    public OnAttack(AttackType attackType, int sequenceID = -1)
    {
        this.AttackType = attackType;
        SequenceID = sequenceID;
    }
    public readonly AttackType AttackType;
    public int SequenceID { get; set; }
}

public enum AttackType
{
     NONE = 0,
     Sword,
     Special
};