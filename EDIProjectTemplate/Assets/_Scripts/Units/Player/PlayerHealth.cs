using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace _Scripts.Units.Player
{
    public class PlayerHealth : Health
    {
        Volume _postProcessing;
       // CameraManager _cameraManager; //for kill cam later on
       
       public Image currentHealthBar;
       public Image currentHealthGlobe;
       
       public bool Regenerate = true;
       public float regen = 0.1f;
       private float timeleft = 0.0f;	// Left time for current interval
       public float regenUpdateInterval = 1f;
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
            UpdateGraphics();
        }

        private void Update()
        {
            if (Regenerate)
            {
                Regen();
            }
        }

        private void OnEnable()
        {
            currentHealthBar.gameObject.SetActive(false);
            currentHealthGlobe.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            currentHealthBar.gameObject.SetActive(true);
            currentHealthGlobe.gameObject.SetActive(false);

        }

        private void Regen()
        {
            timeleft -= Time.deltaTime;

            if (timeleft <= 0.0) // Interval ended - update health & mana and start new interval
            {
                HealDamage(regen);
                UpdateGraphics();

                timeleft = regenUpdateInterval;
            }
        }

        private void UpdateGraphics()
        {
            UpdateHealthBar();
            UpdateHealthGlobe();
        }
        private void UpdateHealthBar()
        {
            float ratio = currentHealth / maxHealth;
            currentHealthBar.rectTransform.localPosition = new Vector3(currentHealthBar.rectTransform.rect.width * ratio - currentHealthBar.rectTransform.rect.width, 0, 0);
            //healthText.text = currentHealth.ToString ("0") + "/" + maxHealth.ToString ("0");
        }

        private void UpdateHealthGlobe()
        {
            float ratio = currentHealth / maxHealth;
            currentHealthGlobe.rectTransform.localPosition = new Vector3(0, currentHealthGlobe.rectTransform.rect.height * ratio - currentHealthGlobe.rectTransform.rect.height, 0);
            //healthText.text = currentHealth.ToString("0") + "/" + maxHealth.ToString("0");
        }
        
        public void HealDamage(float Heal)
        {
            currentHealth += Heal;
            if (currentHealth > maxHealth) 
                currentHealth = maxHealth;

            UpdateGraphics();
        }
    }
}