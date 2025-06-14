public class OnAttack : IEvent
{
    public OnAttack(bool isAttacking, AttackType attackType = AttackType.NONE)
    {
        this.AttackType = attackType;
        this.IsAttacking = isAttacking;
    }
    public AttackType AttackType;
    public bool IsAttacking;
}

public enum AttackType
{
     NONE = 0,
     Sword,
     Special
};