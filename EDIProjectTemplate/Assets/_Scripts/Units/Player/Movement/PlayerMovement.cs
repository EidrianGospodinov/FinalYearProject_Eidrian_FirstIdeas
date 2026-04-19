using System;
using System.Collections.Generic;
using _Scripts.Units.Sound.Footstep;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Transform cameraTransform;
    [SerializeField] private bool faceMoveDirection = false;
    [SerializeField] private Stats playerStats;

    [Header("Controller")]
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1.2f;

    private float runSpeed;
    private Vector3 _PlayerVelocity;
    private bool isGrounded;
    private Vector2 currentMovementInput;
    
    private Vector3 currentWorldMoveDirection;
    private CheckForTerrain checkForTerrain; 
    private List<AudioClip> footstepSounds = new List<AudioClip>();
    private AudioClip jumpSound;
    private AudioClip landSound;
    private AudioSource audioSource;
    private float stepCycle;
    private float nextStep;
    private float stepInterval = .5f;
    private bool wasGrounded;
    

   
    public bool IsMoving => currentMovementInput.magnitude > 0.1f;
    public bool IsJumping => !isGrounded;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        runSpeed = playerStats.GetStat(Stat.RunSpeed);
        playerStats.upgradeApplied += UpgradeApplied;
        checkForTerrain = GetComponent<CheckForTerrain>();
        audioSource = GetComponent<AudioSource>();
        stepCycle = 0f;
        nextStep = stepCycle / 2f;
    }

    private void UpgradeApplied(Stats stats, StatsUpgrade upgrade)
    {
        runSpeed = playerStats.GetStat(Stat.RunSpeed);
    }


    public Vector3 GetWorldMoveDirection()
    {
        // Return the normalized direction vector.
        return currentWorldMoveDirection.normalized;
    }

    // Called by PlayerController in FixedUpdate()
    public void HandlePhysics(Vector2 input, bool isRunning = false)
    {
        if (!wasGrounded && isGrounded)
        {
            PlayLandSound();
        }

        wasGrounded = isGrounded;
        currentMovementInput = input;
        isGrounded = controller.isGrounded;
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        //movement
        Vector3 moveDirection = right * currentMovementInput.x + forward * currentMovementInput.y;
        
        currentWorldMoveDirection = moveDirection;
        
        var playerSpeed = isRunning ? runSpeed : walkSpeed;
        controller.Move(moveDirection * playerSpeed * Time.deltaTime);
        if (faceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
        
        _PlayerVelocity.y += gravity * Time.deltaTime;
        
        if(isGrounded && _PlayerVelocity.y < 0)
        {
            _PlayerVelocity.y = -2f;
        }

        controller.Move(_PlayerVelocity * Time.deltaTime);
        ProgressStepCycle(playerSpeed, isRunning);
    }
    private void ProgressStepCycle(float speed, bool isRunning)
    {
        if (!isGrounded)
        {
            return;
        }
        if (controller.velocity.sqrMagnitude > 0 && (currentMovementInput.x != 0 || currentMovementInput.y != 0))
        {
            stepCycle += (controller.velocity.magnitude + (speed * (isRunning ? 0.7f : 1f))) *
                           Time.fixedDeltaTime;
        }
        if (!(stepCycle > nextStep))
        {
            return;
        }

        nextStep = stepCycle + stepInterval;

        PlayFootStepAudio();
    }
    // Called by PlayerController via input binding
    public void Jump()
    {
        if (isGrounded)
        {
            PlayJumpSound();
            _PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    private void PlayJumpSound()
    {
        checkForTerrain.CheckLayers();
        audioSource.clip = jumpSound;
        audioSource.Play();
    }
    private void PlayLandSound()
    {
        checkForTerrain.CheckLayers();
        audioSource.clip = landSound;
        audioSource.PlayOneShot(landSound);
        nextStep = stepCycle + .5f;
    }


    private void PlayFootStepAudio()
    {
        checkForTerrain.CheckLayers();
        if (!isGrounded)
        {
            return;
        }

        //from Standard Assets Character UNITY
        if (footstepSounds == null || footstepSounds.Count == 0) return;
        int n = Random.Range(1, footstepSounds.Count);
        audioSource.clip = footstepSounds[n];
        audioSource.pitch = Random.Range(0.92f, 1.08f);
        
        audioSource.PlayOneShot(audioSource.clip);

    }
    public void SwapCollection(FootstepCollection collection)
    {
        footstepSounds.Clear();
        for (int i = 0; i < collection.FootstepSounds.Count; i++)
        {
            footstepSounds.Add(collection.FootstepSounds[i]);
        }

        jumpSound = collection.JumpSound;
        landSound = collection.LandSound;
    }
}