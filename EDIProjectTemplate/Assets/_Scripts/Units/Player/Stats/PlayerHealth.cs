using System;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Player.Core;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Units.Player
{
    public class PlayerHealth : Health
    {
        [Inject] private ICameraService cameraService;
        Volume _postProcessing;
        Vignette vignette;
       // CameraManager _cameraManager; //for kill cam later on
       
       public Image currentHealthBar;
       public Image currentHealthGlobe;
       
       public bool canRegenerate = true;
       private float timeleft = 0.0f;	// Left time for current interval
       private float regenUpdateInterval = 1f;
       private EventBinding<OnSwitchHeroEvent> playerEventBinding;
       private PlayerController playerController;
       
       
        protected override void OnStart()
        {
            playerEventBinding = EventBus<OnSwitchHeroEvent>.Register(HandleHeroSwitchEvent);
            _postProcessing = FindFirstObjectByType<Volume>();
            _postProcessing.profile.TryGet(out vignette);
            playerController = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            if (shouldBlink)
            {
                UpdateVignetteEffect();
            }
        }

        private void HandleHeroSwitchEvent(OnSwitchHeroEvent obj) => UpdateGraphics();

        protected override void OnDeath()
        {
           cameraService.EnableDeathCam(true);
           playerController.ActionStateMachine.ChangeState(ActionStateId.Death);
        }
        protected override void OnDamage()
        {
            UpdateGraphics();
        }
        

        private void OnEnable()
        {
            
            currentHealthBar.gameObject.SetActive(false);
            currentHealthGlobe.gameObject.SetActive(true);
        }

        public void ResetHealth()
        {
            isDead = false;
            currentHealth = maxHealth;
            UpdateGraphics();
        }
       

        private void OnDisable()
        {
            currentHealthBar.gameObject.SetActive(true);
            currentHealthGlobe.gameObject.SetActive(false);

        }

        public void Regen(float regen)
        {
            timeleft -= Time.deltaTime;

            if (timeleft <= 0.0)
            {
                HealDamage(regen);
                UpdateGraphics();

                timeleft = regenUpdateInterval;
            }
        }

        public void UpdateGraphics()
        {
            if (gameObject.activeSelf)
            {
                UpdateVignetteEffect();
            }
            UpdateHealthBar();
            UpdateHealthGlobe();
        }

        private bool shouldBlink;
        private void UpdateVignetteEffect()
        {
            if (vignette !=null)
            {
               
                var startVignette = Mathf.Min(maxHealth, maxHealth / 2f + 20f);
                if (startVignette <= currentHealth)
                {
                    vignette.intensity.value = 0;
                    return;
                }
                float percent = 1.0f-(currentHealth / (float)startVignette);
                if (percent > 0.5f)
                {
                    shouldBlink = true;
                    float blink = Mathf.Sin(Time.time * 2f) * 0.2f;
                    percent *= 1f + blink;
                }
                else
                {
                    shouldBlink = false;
                }
                Debug.Log($"percent: {percent}\n current health: {currentHealth} ");
                vignette.intensity.value = Mathf.Clamp(percent, 0, 0.9f);
            }
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