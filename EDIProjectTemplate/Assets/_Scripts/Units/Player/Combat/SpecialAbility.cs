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

    private void OnEnable()
    {
        OnUltimate = EventBus<OnUltimate>.Register(OnUltimateAttackEvent);
        GetUltimateEvent = EventBus<GetUltimateEvent>.Register(OnGetUltimateEvent);
    }

    private void OnGetUltimateEvent(GetUltimateEvent obj)
    {
        index = 1;
        StartCoroutine(Coroutine_Spawn(true));
    }

    private void OnDisable()
    {
        EventBus<OnUltimate>.Unregister(OnUltimate);
        EventBus<GetUltimateEvent>.Unregister(GetUltimateEvent);
    }

    private void OnUltimateAttackEvent(OnUltimate onUltimateData)
    {
#if UNITY_EDITOR
        ClearAllVfx();
        index = 0;
        if (specialAbilityData[0].FindAtTarget)
        {
            var enemyTarget = FindFirstObjectByType<PlayerController>().EnemyDetector.CurrentActiveEnemy;
            
            StartCoroutine(Coroutine_SpawnAtTarget(enemyTarget));
        }
        else
        {
            StartCoroutine(Coroutine_Spawn());
        }
#endif
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
            OnUltimateAttackEvent(null);
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
        
        Transform sourcePoint = IndividualCharacter.BindingPoints.GetBindingPoint(data.Source);
        var vfxData = new VfxData(sourcePoint, IndividualCharacter.GetTarget(), duration, data._Radius);
        vfxData.SetGround(IndividualCharacter.BindingPoints.GetBindingPoint(BindingPointType.Ground));
        
        activeVfxes.Add(go);
        go.Play(vfxData);
    }
    IEnumerator Coroutine_SpawnAtTarget(Transform target, bool asChild = false)
    {
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
        vfxData = new VfxData(target, IndividualCharacter.GetTarget(), duration, data._Radius);
        if (target != null && individualCharacter.HasLineOfSight(target))
        {
            vfxData.SetGround(target/*IndividualCharacter.BindingPoints.GetBindingPoint(BindingPointType.Ground)*/);
            
        }
        Transform sourcePoint = IndividualCharacter.BindingPoints.GetBindingPoint(data.Source);
        
        activeVfxes.Add(go);
        go.Play(vfxData);
    }
}
