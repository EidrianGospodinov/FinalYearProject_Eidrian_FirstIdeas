using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool faceMoveDirection = false;

    [Header("Controller")]
    public float moveSpeed = 5;
    public float gravity = -9.8f;
    public float jumpHeight = 1.2f;

    private Vector3 _PlayerVelocity;
    private bool isGrounded;
    private Vector2 currentMovementInput;
    
    private Vector3 currentWorldMoveDirection;

   
    public bool IsMoving => currentMovementInput.magnitude > 0.1f;
    
    void Awake()
    {
        controller = GetComponent<CharacterController>();
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
    public void HandlePhysics() 
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
        
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
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