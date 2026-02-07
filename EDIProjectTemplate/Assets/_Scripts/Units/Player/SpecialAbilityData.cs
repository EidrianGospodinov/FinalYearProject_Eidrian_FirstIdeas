using PixPlays.ElementalVFX;
using UnityEngine;

namespace _Scripts.Units.Player
{
    [CreateAssetMenu(fileName = "SpeacialAbility", menuName = "Game/SpecialAbility Data")]
    public class SpecialAbilityData: ScriptableObject
    {
        public string Name;
        public float VfxSpawnDelay;
        public BindingPointType Source;
        public float _Duration;
        public float _Radius;
        public BaseVfx VFX;
    }
}