using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheckOrigin; // Transform from which the raycast starts
    public float groundCheckDistance = 0.2f; // Distance of the raycast
    public LayerMask groundMask; // LayerMask for ground detection

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpTriggered;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        GroundCheck();
        Move();
        ApplyGravity();
        Jump();

        Input.GetKey("a");
    }
    
    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(groundCheckOrigin.position, Vector3.down, groundCheckDistance, groundMask);
        
        // Reset vertical velocity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
    
    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
    
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        Debug.Log(velocity.y);
    }
    
    private void Jump()
    {
        if (jumpTriggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log(velocity.y);
        }
        jumpTriggered = false;
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpTriggered = true;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the raycast in the Scene view for debugging
        if (groundCheckOrigin != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
        }
    }
}
