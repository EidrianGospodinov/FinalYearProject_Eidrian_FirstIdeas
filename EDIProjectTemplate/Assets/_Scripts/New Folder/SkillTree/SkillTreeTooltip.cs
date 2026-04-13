using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeTooltip : MonoBehaviour
{
    [SerializeField] private Image frameImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI abilityName;
    [SerializeField] private TextMeshProUGUI abilityUpgradeCost;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    [SerializeField] private TextMeshProUGUI currentAmount;

    private string abilityUpgradeIncreaseText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUp(Image frame, Image icon, string name, int cost, float increaseText, string desc,
        bool isPercentage, float statData)
    {
        frameImage = frame;
        iconImage = icon;
        abilityName.text = name;
        abilityUpgradeCost.text = cost.ToString();
        abilityDescription.text = desc;
        abilityUpgradeIncreaseText = increaseText.ToString();
        if (isPercentage)
        {
            abilityUpgradeIncreaseText += "%";
        }

        abilityDescription.text += " <color=yellow>" + abilityUpgradeIncreaseText + "</color>";
        currentAmount.text = $"{statData.ToString()} <color=yellow> + {abilityUpgradeIncreaseText} </color>";


    }
}
