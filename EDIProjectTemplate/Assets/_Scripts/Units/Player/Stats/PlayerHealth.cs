using System;
using _Scripts.StateMachine.PlayerActionStateMachine;
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
        Vignette vignette;
       // CameraManager _cameraManager; //for kill cam later on
       
       public Image currentHealthBar;
       public Image currentHealthGlobe;
       
       public bool Regenerate = true;
       public float regen = 5f;
       private float timeleft = 0.0f;	// Left time for current interval
       public float regenUpdateInterval = 1f;
       private EventBinding<OnSwitchHeroEvent> playerEventBinding;
       private PlayerController playerController;
       
       
       [SerializeField] private Stats playerStats;

        protected override void OnStart()
        {
            playerEventBinding = EventBus<OnSwitchHeroEvent>.Register(HandleHeroSwitchEvent);
            _postProcessing = FindFirstObjectByType<Volume>();
            _postProcessing.profile.TryGet(out vignette);
            maxHealth = playerStats.GetStat(Stat.Health);
            playerController = GetComponentInParent<PlayerController>();
            //_cameraManager = FindFirstObjectByType<CameraManager>();
        }

        private void HandleHeroSwitchEvent(OnSwitchHeroEvent obj) => UpdateGraphics();

        protected override void OnDeath()
        {
           //_cameraManager.EnableKillCam();
           playerController.ActionStateMachine.ChangeState(ActionStateId.Death);
        }
        protected override void OnDamage()
        {
            UpdateGraphics();
        }

        private void Update()
        {
            /*if (Regenerate)
            {
                Regen();
            }*/
        }

        private void OnEnable()
        {
            playerStats.upgradeApplied += UpgradeApplied;
            maxHealth = playerStats.GetStat(Stat.Health);
            
            currentHealthBar.gameObject.SetActive(false);
            currentHealthGlobe.gameObject.SetActive(true);
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            UpdateGraphics();
        }
        private void UpgradeApplied(Stats stats, StatsUpgrade upgrade)
        {
            maxHealth = playerStats.GetStat(Stat.Health);
            print($"upgrade applied. New player health for {gameObject.name} = {maxHealth}");
        }

        private void OnDisable()
        {
            playerStats.upgradeApplied -= UpgradeApplied;
            
            currentHealthBar.gameObject.SetActive(true);
            currentHealthGlobe.gameObject.SetActive(false);

        }

        public void Regen()
        {
            timeleft -= Time.deltaTime;

            if (timeleft <= 0.0)
            {
                HealDamage(regen);
                UpdateGraphics();

                timeleft = regenUpdateInterval;
            }
        }

        private void UpdateGraphics()
        {
            if (gameObject.activeSelf)
            {
                UpdateVignetteEffect();
            }
            UpdateHealthBar();
            UpdateHealthGlobe();
        }

        private void UpdateVignetteEffect()
        {
            if (vignette !=null)
            {
                float percent = 1.0f-(currentHealth / maxHealth);
                vignette.intensity.value = percent;
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