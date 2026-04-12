using System;
using System.Collections;
using System.Collections.Generic;
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
                float damageTaken = 0;
                other.gameObject.TryGetComponent<Health>(out Health health);
                if (health != null)
                {
                    damageTaken = currentComboData.attackDamage;
                    health.TakeDamage(damageTaken);
                    EventBus<OnEnemyHit>.Trigger(new OnEnemyHit(damageTaken));
                    other.gameObject.TryGetComponent<AiAgent>(out AiAgent agent);
                    if (agent != null)
                    {
                        var agentConfig = agent.agentConfig;
                        DynamicTextData data = agentConfig.DynamicTextData;
                        Vector3 surfacePoint = other.ClosestPoint(transform.position);
                        float offsetDistance = 0.5f; 
                        Vector3 dirToPlayer = (transform.position - surfacePoint).normalized;
                        Vector3 destination = surfacePoint + (dirToPlayer * offsetDistance);
                        
                        float roll = UnityEngine.Random.value;
                        if (agentConfig.critChance > 0 && roll <= agentConfig.critChance)
                        {
                            destination.y += 1f;

                            DynamicTextManager.CreateText(destination, "CRIT!", agentConfig.CritData);

                            damageTaken *= 1.5f;

                            destination.y -= 1f;
                        }

                        DynamicTextManager.CreateText(destination, damageTaken.ToString(), data);
                        /*destination.x += (Random.value - 0.5f) / 3f;
                        destination.y += Random.value;
                        destination.z += (Random.value - 0.5f) / 3f;*/
                        
                        
                    }
                }
            }
            _isAttacking = false;
        }
    }

    public void Initialize(AttackData attackData)
    {
       this.attackData = attackData;
    }
}
