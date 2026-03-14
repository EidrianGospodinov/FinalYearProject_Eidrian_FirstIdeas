using UnityEngine;

namespace _Scripts.Units.Player
{
    [CreateAssetMenu(fileName = "NewAttackTypeData", menuName = "Game/Attack Hero Data")]
    public class HeroData : ScriptableObject
    {
        public string heroName;
        
        public GameObject weaponPrefab;
    }
}