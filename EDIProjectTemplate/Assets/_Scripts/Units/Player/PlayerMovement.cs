using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

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

        //movement
        Vector3 moveDirection = new Vector3(currentMovementInput.x, 0, currentMovementInput.y);
        
        currentWorldMoveDirection = transform.TransformDirection(moveDirection);
        
        controller.Move(currentWorldMoveDirection * moveSpeed * Time.deltaTime);
        
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