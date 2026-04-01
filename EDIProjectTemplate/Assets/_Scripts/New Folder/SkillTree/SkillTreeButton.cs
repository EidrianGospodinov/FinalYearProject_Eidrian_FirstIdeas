using _Scripts.New_Folder.SkillTree;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
        [SerializeField] private StatsUpgrade statsUpgrade;
        [SerializeField] public Image frameImage;
        [SerializeField] public SkillLevel skillLevel;
        [SerializeField] private SkillAmount skillAmount;
        [SerializeField] public Color activeFrameColor;
        [SerializeField] public Color disabledFrameColor;
        [SerializeField] public Color activeIconColor;
        [SerializeField] public Color disabledIconColor;

        public void OnButtonPress()
        {
            Increment();
        }
        
        protected void SetupUI()
        {
            SetColors();
        }



        

        private void Increment()
        {
            if (!CanIncrement()) return;

            skillAmount.TrySpend(statsUpgrade.cost);
            skillLevel.IncrementLevel();
            SetColors();
        }
        
        private bool CanIncrement()
        {
            if (skillLevel.IsFullLevels())
            {
                return false;
            }

            if (skillAmount.CanSpend(statsUpgrade.cost))
            {
                statsUpgrade.DoUpgrade();
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
    }
