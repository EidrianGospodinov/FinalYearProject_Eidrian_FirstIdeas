

using _Scripts.Units.Player;
using UnityEngine;

public enum HeroType
{
    HeroA, 
    HeroB  
}
public class HeroSwitcher : MonoBehaviour
{
    public static HeroSwitcher Instance { get; private set; }
    
    
    public HeroType ActiveHero { get; private set; } = HeroType.HeroA;
    public HeroData ActiveHeroData { get; private set; }
    [Header("Hero Data Assets")]
    [SerializeField] private HeroData heroA_Data;
    [SerializeField] private HeroData heroB_Data;

    [Header("Visual Body GameObjects")]
    [SerializeField] private GameObject heroA_Body; 
    [SerializeField] private GameObject heroB_Body;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
        if (heroA_Body != null)
        {
            heroA_Body.SetActive(true);
            heroB_Body.SetActive(false);
            ActiveHeroData = heroA_Data;
            ActiveHero = HeroType.HeroA;
        }
    }
    public void RequestHeroSwitch()
    {
        HeroType newHero = (ActiveHero == HeroType.HeroA) ? HeroType.HeroB : HeroType.HeroA;
        SwitchTo(newHero);
    }

    private void SwitchTo(HeroType newHero)
    {
        if (ActiveHero == newHero) return;
        
        ActiveHero = newHero;
        ActiveHeroData = (newHero == HeroType.HeroA) ? heroA_Data : heroB_Data;
        
        bool isHeroA = (newHero == HeroType.HeroA);
        
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
