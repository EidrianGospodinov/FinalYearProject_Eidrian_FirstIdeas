using _Scripts.Units.Player.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractTutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject inputTutorialPrefab;
    
    private EventBinding<OnUltimate> OnUltimate;
    private EventBinding<GetUltimateEvent> GetUltimateEvent;
    private EventBinding<OnSwitchHeroEvent> SwitchHeroEvent;
    private EventBinding<OnPlayerCriticalHealth> OnPlayerCriticalHealht;


    private InteractTutorial ultimateTutorialGO;
    private InteractTutorial SwitchHeroTutorialGO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnUltimate = EventBus<OnUltimate>.Register(OnUltimateAttackEvent);
        GetUltimateEvent = EventBus<GetUltimateEvent>.Register(OnGetUltimateEvent);
        SwitchHeroEvent = EventBus<OnSwitchHeroEvent>.Register(OnGetSwitchHeroEvent);
        OnPlayerCriticalHealht = EventBus<OnPlayerCriticalHealth>.Register(HandleOnPlayerCriticalHealth);

    }

    private void HandleOnPlayerCriticalHealth(OnPlayerCriticalHealth obj)
    {
        if (SwitchHeroTutorialGO == null)
        {
            SwitchHeroTutorialGO = CreateInputTutorial("Press V");
        }
        else
        {
            Debug.Log("switch hero game object exists");
        }
    }

    private void OnGetSwitchHeroEvent(OnSwitchHeroEvent obj)
    {
        if (SwitchHeroTutorialGO != null)
        {
            SwitchHeroTutorialGO.DestroySelf();
        }
        else
        {
            Debug.Log("hero switch game object does not exist");
        }
    }

    private void OnGetUltimateEvent(GetUltimateEvent obj)
    {
        if (ultimateTutorialGO == null)
        {
            ultimateTutorialGO = CreateInputTutorial("Press X");
        }
        else
        {
            Debug.Log("Ultimate game object exists");
        }
    }

    private void OnUltimateAttackEvent(OnUltimate obj)
    {
        if (ultimateTutorialGO != null)
        {
            ultimateTutorialGO.DestroySelf();
        }
        else
        {
            Debug.Log("Ultimate game object does not exist");
        }
    }

    InteractTutorial CreateInputTutorial(string text)
    {
        Instantiate(inputTutorialPrefab, transform).TryGetComponent<InteractTutorial>(out var interactTutorial);
        if (interactTutorial != null)
        {
            interactTutorial.UpdateTutorialText(text);
            return interactTutorial;
        }

        Debug.Log($"{this.name} instance does not have a prefab with InteractTutorial component");
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
