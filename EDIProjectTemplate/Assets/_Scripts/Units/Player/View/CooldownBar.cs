using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.Units.Player.View
{
    public class CooldownBar : MonoBehaviour
    {
        private enum CooldownConsumeType { Fill, Drain }

        private const float SkillBarMargin = 2f;
        private const string DefaultBarTextFormat = "{0:0.0} sec";
        private CanvasGroup canvasGroup;

        [SerializeField] private Stats playerStats;
        
        [Header("UI References")] 
        [SerializeField] private Image skillBarImage;
        [SerializeField] private Image skillIconImage;
        [SerializeField] private TMP_Text skillNameText;
        [SerializeField] private TMP_Text skillRemainingCooldownText;
        
        [Header("Settings")]
        private float initSkillCooldown = 5f;
        //[SerializeField] private float initSkillCooldownRemaining;
        [SerializeField] private string skillName;
        [SerializeField] private CooldownConsumeType consumeType = CooldownConsumeType.Fill;
        [SerializeField] private bool depleteWhenCompleted;


        private float cooldownRemaining;
        private float cooldown;
        private bool isFinished;
        private bool isCoolingDown;
        private RectTransform rectTransform;
        private RectTransform skillBarImageTransform;

        private EventBinding<OnSwitchHeroEvent> onHeroSwitch;

        public void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            onHeroSwitch = EventBus<OnSwitchHeroEvent>.Register(HandleOnHeroSwitchEvent);
            InitDefaultValues();
            UpdateProgress(); 
        }

        

        private void HandleOnHeroSwitchEvent(OnSwitchHeroEvent obj)
        {
            canvasGroup.alpha = 1f;
            //cooldown = 
            RestartCooldown();
        }

        public void Cancel()
        {
            Deplete();
        }

        public void StartCooldown()
        {
            isCoolingDown = true;
            isFinished = false;
        }

        public void RestartCooldown()
        {
            GetCooldown();
            InitDefaultValues();
            StartCooldown();
        }

        void GetCooldown()
        {
            initSkillCooldown=playerStats.GetStat(Stat.switchHeroCooldown);
        }
        private void InitDefaultValues()
        {

            isFinished = false;
            cooldown = initSkillCooldown;
            cooldownRemaining = initSkillCooldown;
            
            if (skillNameText != null) 
                skillNameText.text = skillName;

            rectTransform = GetComponent<RectTransform>();
            skillBarImageTransform = skillBarImage.GetComponent<RectTransform>();
            
            InitConsumeType();
            SetRemainingCooldownText();
        }

        public void Update()
        {
            if (!isCoolingDown || isFinished) return;

            // Reduce timer
            cooldownRemaining = Math.Max(0, cooldownRemaining - Time.deltaTime);
            
            // Update UI visuals
            UpdateProgress();

            // Check if for timer end
            if (cooldownRemaining <= 0)
            {
                SkillFinished();
            }
        }

        public bool IsNotInUse() => !isCoolingDown || isFinished;

        private void InitConsumeType()
        {
            var scale = consumeType == CooldownConsumeType.Drain ? -1 : 1;
            skillBarImageTransform.localScale = new Vector3(scale, 1, 1);
        }

        private void UpdateProgress()
        {
            SetRemainingCooldownText();
            SetBarLength();
        }
        
        private void SkillFinished()
        {
            isFinished = true;
            isCoolingDown = false;
            
            if (depleteWhenCompleted)
                Deplete();
        }

        private void Deplete()
        {
            canvasGroup.alpha = 0f;
        }

        private void SetRemainingCooldownText()
        {
            if (skillRemainingCooldownText != null)
                skillRemainingCooldownText.text = string.Format(DefaultBarTextFormat, cooldownRemaining);
        }

        private void SetBarLength()
        {
            if (consumeType == CooldownConsumeType.Fill)
                FillBar();
            else
                DrainBar();
        }

        private void FillBar()
        {
            float progressPercentage = CurrentPercentage();
            float barWidth = GetBarWidth();
            float currentWidth = barWidth * progressPercentage;
            
            skillBarImageTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, SkillBarMargin, currentWidth);
        }

        private void DrainBar()
        {
            float progressPercentage = 1f - CurrentPercentage();
            float barWidth = GetBarWidth();
            float currentWidth = barWidth * progressPercentage;
            
            skillBarImageTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, SkillBarMargin, currentWidth);
        }

        private float CurrentPercentage() 
        {
            if (cooldown <= 0) return 1f;
            return Mathf.Clamp01((cooldown - cooldownRemaining) / cooldown);
        }

        private float GetBarWidth() => rectTransform.rect.width - (SkillBarMargin * 2);
    }
}
