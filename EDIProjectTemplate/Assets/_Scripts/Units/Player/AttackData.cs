using UnityEngine;

namespace _Scripts.Units.Player
{
    [CreateAssetMenu(fileName = "NewAttackData", menuName = "Game/Attack Data")]
    public class AttackData : ScriptableObject
    {
        [Header("Attacking")]
        public float attackDistance = 3f;
        public float attackDelay = 0.4f;
        public float attackSpeed = 1f;
        public int attackDamage = 1;
        public LayerMask attackLayer;

        [Header("Dash")]
        public float dashCooldownDuration = 1f;
        public float dashSpeed = 7f;
        [Header("Visual and sound effects")]
        public GameObject hitVfxPrefab;
        public AudioClip swordSwing;
        public AudioClip impactSound;
        
        public GameObject WeaponPrefab;
    }
}