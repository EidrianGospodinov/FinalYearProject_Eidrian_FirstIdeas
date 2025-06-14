using System;
using System.Collections;
using System.Collections.Generic;
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

    private EventBinding<TestEvent> eventBinding;
    private EventBinding<PlayerEvent> playerEventBinding;

    private void OnEnable()
    {
        eventBinding = new EventBinding<TestEvent>(HandleTestEvent);
        EventBus<TestEvent>.Register(eventBinding);

        playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
        EventBus<PlayerEvent>.Register(playerEventBinding);


    }

    private void OnDisable()
    {
        EventBus<TestEvent>.Unregister(eventBinding);
        EventBus<PlayerEvent>.Unregister(playerEventBinding);
    }

    

    private void HandlePlayerEvent(PlayerEvent obj)
    {
        print(obj.PlayerID);
    }

    private void HandleTestEvent(TestEvent obj)
    {
        print("success");
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
          if(playerState != PlayerState.Attacking) return;
        Debug.Log("before hit");
        if (other.gameObject.layer == LayerMask.NameToLayer(layerName))
        {
            Debug.Log("OnHit");
            audioSource.pitch = 1;
            audioSource.PlayOneShot(hitSound);

            GameObject GO = Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(GO, 20);
        }
    }
}
