using _Scripts.StateMachine.PlayerActionStateMachine;
using UnityEngine;

namespace _Scripts.Units.Player
{
    [CreateAssetMenu(fileName = "NewAttackTypeData", menuName = "Game/Attack Type Data")]
    public class AttackComboData : ScriptableObject
    {
        public string attackName = "Basic Attack";

        public ComboStateId comboStateId;

        public float attackDelay = 0.4f;
        public int attackDamage = 1;
        public string animationName;
        
        public AudioClip swordSwing;
        
    }
}