using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Enemy;
using _Scripts.Units.Player;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class OnHit : MonoBehaviour
{
    
    [SerializeField] LayerMask layerMask;
    [Tooltip("Single layer only")]
    public string layerName;

    [SerializeField] private AudioSource audioSource;
    public AudioClip hitSound;
    public GameObject hitEffect;

    //[Inject]PlayerState playerState;
    
    //[Inject]IEventBus<IEvent> eventBus;
    [Inject] private DynamicTextServices dynamicTextServices;

    private EventBinding<OnAttack> OnAttack;
    private EventBinding<PlayerEvent> playerEventBinding;

    private bool _isAttacking = false;
    private AttackComboData currentComboData;
    private AttackData attackData;

    private void OnEnable()
    {
        OnAttack = EventBus<OnAttack>.Register(OnAttackEvent);

        playerEventBinding = EventBus<PlayerEvent>.Register(HandlePlayerEvent);


    }

    private void OnDisable()
    {
        EventBus<OnAttack>.Unregister(OnAttack);
        EventBus<PlayerEvent>.Unregister(playerEventBinding);
    }

    

    private void HandlePlayerEvent(PlayerEvent obj)
    {
        print(obj.PlayerID);
    }

    private void OnAttackEvent(OnAttack evt)
    {
        switch (evt.AttackType)
        {
            case AttackType.NONE:
            case AttackType.LongRange:
                _isAttacking = false;
                break;
            case AttackType.Sword:
                _isAttacking = true;
                break;
        }

        if (evt.ComboStateId == ComboStateId.WindDown || attackData == null) 
        {
            return;
        }
        currentComboData =  attackData.GetComboStateId(evt.ComboStateId);
        
        
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!_isAttacking) return;
        Debug.Log("before hit");
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            Debug.Log("OnHit");
            audioSource.pitch = 1;
            audioSource.PlayOneShot(hitSound);

            GameObject GO = Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(GO, 20);
            if (currentComboData != null)
            {
                float damageTaken = currentComboData.attackDamage;
                other.gameObject.TryGetComponent<AiAgent>(out AiAgent agent);
                other.gameObject.TryGetComponent<Health>(out Health health);
                if (agent == null || health == null)
                {
                    return;
                }

                damageTaken = dynamicTextServices.HandleDamageVisuals(transform, other, agent, damageTaken, true);
                if (health != null)
                {
                    EventBus<OnEnemyHit>.Trigger(new OnEnemyHit(damageTaken, health));
                    health.TakeDamage(damageTaken);
                }
            }
            _isAttacking = false;
        }
    }

    /*private float HandleDamageVisuals(Collider other, AiAgent agent, float damageTaken)
    {
        var agentConfig = agent.agentConfig;
        DynamicTextData data = agentConfig.DynamicTextData;
        Vector3 surfacePoint = other.ClosestPoint(transform.position);
        float offsetDistance = 0.5f; 
        Vector3 dirToPlayer = (transform.position - surfacePoint).normalized;
        Vector3 destination = surfacePoint + (dirToPlayer * offsetDistance);
                        
        HandleCritLogic(ref damageTaken, agentConfig, ref destination);

        DynamicTextManager.CreateText(destination, damageTaken.ToString(), data);
        return damageTaken;
    }

    private void HandleCritLogic(ref float damageTaken, AiAgentConfig agentConfig, ref Vector3 destination)
    {
        float roll = UnityEngine.Random.value;
        if (agentConfig.critChance > 0 && roll <= agentConfig.critChance)
        {
            DynamicTextManager.CreateText(destination + Vector3.up, "CRIT!", agentConfig.CritData);
            damageTaken *= 1.5f;
        }
    }*/

    public void Initialize(AttackData attackData)
    {
       this.attackData = attackData;
    }
}
