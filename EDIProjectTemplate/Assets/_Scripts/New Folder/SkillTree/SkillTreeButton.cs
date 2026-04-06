using System;
using System.Collections.Generic;
using _Scripts.New_Folder.SkillTree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
        [SerializeField] private List<StatsUpgrade> statsUpgrade;
        [SerializeField] public Image frameImage;
        [SerializeField] public Image iconImage;
        [SerializeField] public SkillLevel skillLevel;
        [SerializeField] private SkillAmount skillAmount;
        [SerializeField] public Color activeFrameColor;
        [SerializeField] public Color disabledFrameColor;
        [SerializeField] public Color activeIconColor;
        [SerializeField] public Color disabledIconColor;

        [SerializeField] private GameObject tooltip;

        public void OnButtonPress()
        {
            Increment();
        }

        private void Start()
        {
            skillLevel.SetMaxLevel(statsUpgrade.Count);
            SetupUI();
            SetUpTooltip();
        }

         void SetupUI()
        {
            SetIconImage();
            SetColors();
        }

        private void SetUpTooltip()
        {
            var skillTreeTooltip = tooltip.GetComponent<SkillTreeTooltip>();

            if (skillTreeTooltip != null)
            {
                var statUpgrade =statsUpgrade[skillLevel.GetCurrentLevel()];
                skillTreeTooltip.SetUp(frameImage, iconImage, statUpgrade.upgradeName, statUpgrade.cost, statUpgrade.UpgradeToApply[0].statValue, statUpgrade.description, statUpgrade.isPercentUpgrade  );
            }
        }



        

        private void Increment()
        {
            if (!CanIncrement()) return;

            var result = skillAmount.TrySpend(statsUpgrade[skillLevel.GetCurrentLevel()].cost);
            if (result)
            {
                statsUpgrade[skillLevel.GetCurrentLevel()].DoUpgrade();
                skillLevel.IncrementLevel();
                SetupUI();
                SetUpTooltip();
            }
        }
        
        private bool CanIncrement()
        {
            if (skillLevel.IsFullLevels())
            {
                return false;
            }

            if (skillAmount.CanSpend(statsUpgrade[skillLevel.GetCurrentLevel()].cost))
            {
                return true;
            }

            return false;
        }


        private void SetColors()
        {
            if (skillLevel.HaveLevels())
            {
                frameImage.color = activeFrameColor;
                
            }
            else
            {
                frameImage.color = disabledFrameColor;
            }
        }

        private void SetIconImage()
        {
            iconImage.sprite = statsUpgrade[skillLevel.GetCurrentLevel()].icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltip != null)
            {
                SetUpTooltip();
                tooltip.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(tooltip!=null)
                tooltip.SetActive(false);
        }

        private void OnDisable()
        {
            tooltip.SetActive(false);
        }
}
