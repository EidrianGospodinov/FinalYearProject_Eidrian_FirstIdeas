

using System;
using _Scripts.Units.Player;
using UnityEngine;

public enum HeroType
{
    Oreon, 
    Thalia  
}
public class HeroSwitcher : MonoBehaviour
{
    
    
    public HeroType ActiveHero { get; private set; } = HeroType.Oreon;
    public HeroData ActiveHeroData { get; private set; }
    [Header("Hero Data Assets")]
    [SerializeField] private HeroData heroA_Data;
    [SerializeField] private HeroData heroB_Data;

    [Header("Visual Body GameObjects")]
    [SerializeField] private GameObject heroA_Body; 
    [SerializeField] private GameObject heroB_Body;

    

    private void Start()
    {
        if (heroA_Body != null)
        {
            heroA_Body.SetActive(true);
            heroB_Body.SetActive(false);
            ActiveHeroData = heroA_Data;
            ActiveHero = HeroType.Oreon;
        }
    }

    public void RequestHeroSwitch()
    {
        HeroType newHero = (ActiveHero == HeroType.Oreon) ? HeroType.Thalia : HeroType.Oreon;
        SwitchTo(newHero);
    }

    private void SwitchTo(HeroType newHero)
    {
        if (ActiveHero == newHero) return;
        
        ActiveHero = newHero;
        ActiveHeroData = (newHero == HeroType.Oreon) ? heroA_Data : heroB_Data;
        
        bool isHeroA = (newHero == HeroType.Oreon);
        
        heroA_Body.SetActive(isHeroA);
        heroB_Body.SetActive(!isHeroA);
        
       
        
       
        EventBus<OnSwitchHeroEvent>.Trigger(new OnSwitchHeroEvent(ActiveHeroData));
        Debug.Log($"Hero switched to: {newHero}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
