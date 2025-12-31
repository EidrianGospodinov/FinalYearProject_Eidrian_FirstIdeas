using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace _Scripts.Units.Player
{
    public class PlayerHealth : Health
    {
        PostProcessVolume _postProcessing;
       // CameraManager _cameraManager; //for kill cam later on
        protected override void OnStart()
        {
            _postProcessing = FindFirstObjectByType<PostProcessVolume>();
            //_cameraManager = FindFirstObjectByType<CameraManager>();
        }
        protected override void OnDeath()
        {
           //_cameraManager.EnableKillCam();
        }
        protected override void OnDamage()
        {
            Vignette vignette;
        
            if (_postProcessing.profile.TryGetSettings(out vignette))
            {
                float percent = 1.0f-(currentHealth / maxHealth);
                vignette.intensity.value = percent;
            }
        }
    }
}