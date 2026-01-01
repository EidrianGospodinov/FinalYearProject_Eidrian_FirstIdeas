using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Scripts.Units.Player
{
    public class PlayerHealth : Health
    {
        Volume _postProcessing;
       // CameraManager _cameraManager; //for kill cam later on
        protected override void OnStart()
        {
            _postProcessing = FindFirstObjectByType<Volume>();
            //_cameraManager = FindFirstObjectByType<CameraManager>();
        }
        protected override void OnDeath()
        {
           //_cameraManager.EnableKillCam();
        }
        protected override void OnDamage()
        {
            Vignette vignette;
        
            if (_postProcessing.profile.TryGet(out vignette))
            {
                float percent = 1.0f-(currentHealth / maxHealth);
                vignette.intensity.value = percent;
            }
        }
    }
}