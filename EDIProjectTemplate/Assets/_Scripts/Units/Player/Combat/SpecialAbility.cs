using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Units.Player;
using PixPlays.ElementalVFX;
using UnityEngine;
using UnityEngine.Serialization;

public class SpecialAbility : MonoBehaviour
{
    /// <summary>
    /// slot zero - special attack
    /// slot 1 - ability beam
    /// </summary>
    [SerializeField] private List<SpecialVFXData> specialAbilityData;
    [FormerlySerializedAs("_Character")] [SerializeField] IndividualCharacter individualCharacter;

    public IndividualCharacter IndividualCharacter => individualCharacter;

    [HideInInspector][SerializeField] string _CurrentData;

    private int index = 0;
    private List<BaseVfx> activeVfxes = new List<BaseVfx>();
    
    
    private EventBinding<OnUltimate> OnUltimate;
    private EventBinding<GetUltimateEvent> GetUltimateEvent;
    private EventBinding<OnLongRange> OnLongRange;

    private void OnEnable()
    {
        OnUltimate = EventBus<OnUltimate>.Register(OnUltimateAttackEvent);
        GetUltimateEvent = EventBus<GetUltimateEvent>.Register(OnGetUltimateEvent);
        OnLongRange = EventBus<OnLongRange>.Register(OnGetLongRangeEvent);
    }

    private void OnGetLongRangeEvent(OnLongRange OnLongRangeData)
    {
        index = 2;
        ApplyVisualEffect(OnLongRangeData.EnemyTarget);
    }

    private void OnGetUltimateEvent(GetUltimateEvent GetUltimateEvent)
    {
        index = 1;
        StopCoroutine(Coroutine_Spawn(null,true));
        StartCoroutine(Coroutine_Spawn(null,true));
    }

    private void OnDisable()
    {
        EventBus<OnUltimate>.Unregister(OnUltimate);
        EventBus<GetUltimateEvent>.Unregister(GetUltimateEvent);
        EventBus<OnLongRange>.Unregister(OnLongRange);
    }

    private void OnUltimateAttackEvent(OnUltimate onUltimateData)
    {
        ClearAllVfx();
        index = 0;
        ApplyVisualEffect(onUltimateData.target);
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
#if UNITY_EDITOR
        var enemyTarget = FindFirstObjectByType<PlayerController>().EnemyDetector.CurrentActiveEnemy;
        if (Input.GetKeyDown(KeyCode.O))
        {
            index = 0;
            ApplyVisualEffect(enemyTarget);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            index = 1;
            StopCoroutine(Coroutine_Spawn(enemyTarget));
            StartCoroutine(Coroutine_Spawn(enemyTarget,true));
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            index = 2;
            ApplyVisualEffect(enemyTarget);
        }
#endif
    }

    private void ApplyVisualEffect(Transform enemyTarget)
    {
        if (specialAbilityData[index].FindAtTarget)
        {
            StartCoroutine(Coroutine_SpawnAtTarget(enemyTarget));
        }
        else
        {
            StartCoroutine(Coroutine_Spawn(enemyTarget));
        }
    }

    IEnumerator Coroutine_Spawn(Transform target, bool asChild = false)
    {
        if (target == null)
        {
            target = IndividualCharacter.GetTargetFallback();
        }
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
        
        Transform sourcePoint = IndividualCharacter.BindingPoints.GetBindingPoint(data.Source);
        var vfxData = new VfxData(sourcePoint, target, duration, data._Radius);
        vfxData.SetGround(IndividualCharacter.BindingPoints.GetBindingPoint(BindingPointType.Ground));
        
        activeVfxes.Add(go);
        go.Play(vfxData);
    }
    IEnumerator Coroutine_SpawnAtTarget(Transform target, bool asChild = false)
    {
        //todo: this function needs clean up
        if (target == null)
        {
            target = IndividualCharacter.GetTargetFallback();
        }
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
        
        
        //Transform target = individualCharacter.GetClosestEnemy(data._Radius);
        VfxData vfxData;
        vfxData = new VfxData(target, target, duration, data._Radius);
        if (target != null && individualCharacter.HasLineOfSight(target))
        {
            vfxData.SetGround(target/*IndividualCharacter.BindingPoints.GetBindingPoint(BindingPointType.Ground)*/);
            
        }
        Transform sourcePoint = IndividualCharacter.BindingPoints.GetBindingPoint(data.Source);
        
        activeVfxes.Add(go);
        go.Play(vfxData);
    }
}
