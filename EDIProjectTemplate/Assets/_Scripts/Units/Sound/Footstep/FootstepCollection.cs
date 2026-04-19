using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Units.Sound.Footstep
{
    [CreateAssetMenu(menuName = "Create New Footstep Collection/New Footstep Collection")]
    public class FootstepCollection : ScriptableObject
    {
        public List<AudioClip> FootstepSounds = new List<AudioClip>();
        public AudioClip JumpSound;
        public AudioClip LandSound;
    }
}