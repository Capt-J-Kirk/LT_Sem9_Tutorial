using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpHeight = 3;
    private readonly float gravity = -9.82f;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 gravityVector;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        // Horizontal movement, based on input.
        Vector3 moveVector = new(moveInput.x, 0, moveInput.y);
        characterController.Move(moveSpeed * Time.deltaTime * moveVector);

        // Apply gravity
        if (!characterController.isGrounded) {
            if (gravityVector.magnitude < Mathf.Abs(gravity) ||
                characterController.gameObject.transform.position.y > 1)
              gravityVector.y += gravity * Time.deltaTime;
            characterController.Move(gravityVector * Time.deltaTime);
        }
        else gravityVector = Vector3.zero;
    }
    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context) {
        if (context.performed && characterController.isGrounded)
            gravityVector.y = Mathf.Sqrt(-gravity * jumpHeight * 1.8f);
    }
}