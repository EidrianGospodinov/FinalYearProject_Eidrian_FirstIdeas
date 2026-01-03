using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.StateMachine.PlayerActionStateMachine;
using _Scripts.Units.Player;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class OnHit : MonoBehaviour
{
    
    [SerializeField] LayerMask layerMask;
    [Tooltip("Single layer only")]
    public string layerName;

    [SerializeField] private AudioSource audioSource;
    public AudioClip hitSound;
    public GameObject hitEffect;

    [Inject]PlayerState playerState;
    
    [Inject]IEventBus<IEvent> eventBus;

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
            if (attackData != null)
            {
                other.gameObject.TryGetComponent<Health>(out Health health);
                if (health != null)
                {
                    health.TakeDamage(attackData.attackDamage);
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
