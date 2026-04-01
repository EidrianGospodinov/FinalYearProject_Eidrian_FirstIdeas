using _Scripts.New_Folder.SkillTree;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    [SerializeField] private SkillAmount skillAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        skillAmount.UpdateSkillAmountText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
