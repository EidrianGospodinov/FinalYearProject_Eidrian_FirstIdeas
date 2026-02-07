using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Units.Player;
using PixPlays.ElementalVFX;
using UnityEngine;

public class SpecialAbility : MonoBehaviour
{
    /// <summary>
    /// slot zero - special attack
    /// slot 1 - ability beam
    /// </summary>
    [SerializeField] private List<SpecialVFXData> specialAbilityData;
    [SerializeField] Character _Character;

    public Character Character => _Character;

    [HideInInspector][SerializeField] string _CurrentData;

    private int index = 0;
    private List<BaseVfx> activeVfxes = new List<BaseVfx>();
    
    
    private EventBinding<OnUltimate> OnUltimate;
    private EventBinding<GetUltimateEvent> GetUltimateEvent;

    private void Start()
    {
        OnUltimate = EventBus<OnUltimate>.Register(OnUltimateAttackEvent);
        GetUltimateEvent = EventBus<GetUltimateEvent>.Register(OnGetUltimateEvent);
    }

    private void OnGetUltimateEvent(GetUltimateEvent obj)
    {
        index = 1;
        StartCoroutine(Coroutine_Spawn(true));
    }

    private void OnDestroy()
    {
        EventBus<OnUltimate>.Unregister(OnUltimate);
    }

    private void OnUltimateAttackEvent(OnUltimate obj)
    {
        ClearAllVfx();
        index = 0;
        StartCoroutine(Coroutine_Spawn());
    }
    private void ClearAllVfx()
    {
        for (int i = activeVfxes.Count - 1; i >= 0; i--)
        {
            if (activeVfxes[i] != null) 
                Destroy(activeVfxes[i].gameObject);
        }
    
        activeVfxes.Clear();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            index = 0;
            StartCoroutine(Coroutine_Spawn());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            index = 1;
            StartCoroutine(Coroutine_Spawn(true));
        }
    }

    IEnumerator Coroutine_Spawn(bool asChild = false)
    {
        //Character.PlayAnimation("New Animation", specialAbilityData[index].clip);
        var data = specialAbilityData[index];
        yield return new WaitForSeconds(data.VfxSpawnDelay);
        BaseVfx go;
        if (asChild)
        {
            go = Instantiate(data.VFX, this.transform);
        }
        else
        {
            go = Instantiate(data.VFX);
        }

        float duration = data._Duration <= 0 ? float.MaxValue : data._Duration;
        
        Transform sourcePoint = Character.BindingPoints.GetBindingPoint(data.Source);
        var vfxData = new VfxData(sourcePoint, Character.GetTarget(), duration, data._Radius);
        vfxData.SetGround(Character.BindingPoints.GetBindingPoint(BindingPointType.Ground));
        
        activeVfxes.Add(go);
        go.Play(vfxData);
    }
}
