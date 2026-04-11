using System;
using UnityEngine;
using UnityEngine.UI;

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
    }

    // Called by PlayerController in Update()
    public void SetMovementInput(Vector2 input)
    {
        currentMovementInput = input;
    }
    public Vector3 GetWorldMoveDirection()
    {
        // Return the normalized direction vector.
        return currentWorldMoveDirection.normalized;
    }

    // Called by PlayerController in FixedUpdate()
    public void HandlePhysics(bool isRunning = false) 
    { 
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
    }

    // Called by PlayerController via input binding
    public void Jump()
    {
        if (isGrounded)
        {
            _PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}