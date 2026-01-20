using System.Collections.Generic;
using _Scripts.StateMachine.PlayerActionStateMachine;
using UnityEngine;

namespace _Scripts.Units.Player
{
    [CreateAssetMenu(fileName = "NewAttackData", menuName = "Game/Attack Data")]
    public class AttackData : ScriptableObject
    {
        [Header("Attacking")]
        //public float attackDistance = 3f;
        /*public float attackDelay = 0.4f;
        public float thirdAttackDelay = 1.3f;*/
        public float attackSpeed = 1f;
        public int attackDamage = 1;
        public LayerMask attackLayer;
        public float powerUpXpRequired;

        [Header("Dash")]
        public float dashCooldownDuration = 1f;
        public float dashSpeed = 7f;
        [Header("Visual and sound effects")]
        public GameObject hitVfxPrefab;
        public AudioClip swordSwing;
        public AudioClip impactSound;
        
        public GameObject WeaponPrefab;
        public List<AttackComboData> attackComboList;

        public AttackComboData GetComboStateId(ComboStateId comboStateId)
        {
            return attackComboList.Find(x => x.comboStateId == comboStateId);
        }
    }

    [CreateAssetMenu(fileName = "NewAttackTypeData", menuName = "Game/Attack Hero Data")]
    public class HeroData : ScriptableObject
    {
        public string heroName;
        
        public GameObject weaponPrefab;
    }
}