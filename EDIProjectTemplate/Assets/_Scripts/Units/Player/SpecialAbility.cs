using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Units.Player;
using PixPlays.ElementalVFX;
using UnityEngine;

public class SpecialAbility : MonoBehaviour
{
    [SerializeField] private List<SpecialAbilityData> specialAbilityData;
    [SerializeField] Character _Character;

    public Character Character => _Character;

    [HideInInspector][SerializeField] string _CurrentData;

    private int index = 0;
    
    private EventBinding<OnUltimate> OnUltimate;

    private void Start()
    {
        OnUltimate = EventBus<OnUltimate>.Register(OnUltimateAttackEvent);
    }

    private void OnUltimateAttackEvent(OnUltimate obj)
    {
        StartCoroutine(Coroutine_Spanw());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index--;
            if (index < 0)
            {
                index = specialAbilityData.Count - 1;
            }
            _CurrentData = specialAbilityData[index].VFX.name;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            index++;
            if (index >= specialAbilityData.Count)
            {
                index = 0;
            }
            _CurrentData = specialAbilityData[index].VFX.name;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Coroutine_Spanw());
        }
    }

    IEnumerator Coroutine_Spanw()
    {
        //Character.PlayAnimation("New Animation", specialAbilityData[index].clip);
        yield return new WaitForSeconds(specialAbilityData[index].VfxSpawnDelay);
        BaseVfx go = Instantiate(specialAbilityData[index].VFX);
        Transform sourcePoint = Character.BindingPoints.GetBindingPoint(specialAbilityData[index].Source);
        var vfxData = new VfxData(sourcePoint, Character.GetTarget(), specialAbilityData[index]._Duration, specialAbilityData[index]._Radius);
        vfxData.SetGround(Character.BindingPoints.GetBindingPoint(BindingPointType.Ground));
        go.Play(vfxData);
    }
}
