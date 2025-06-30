using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControllerCharacter : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2.5f;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    private float originalHeight;
    private bool isCrouching = false;

    [Header("Look")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public float cameraCrouchOffset = -0.5f;

    [Header("Gravity & Ground")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    private CharacterController controller;
    private float xRotation = 0f;
    private MovementState currentState = MovementState.Walking;
    private MovementState previousState = MovementState.Walking;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
    }

    void Update()
    {
        HandleLook();
        HandleCrouchToggle();
        HandleMovement();
        ApplyGravity();
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        float speed;
        if (isCrouching)
        {
            speed = crouchSpeed;
            currentState = MovementState.Crouching;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
            currentState = MovementState.Sprinting;
        }
        else
        {
            speed = walkSpeed;
            currentState = MovementState.Walking;
        }

        controller.Move(move * speed * Time.deltaTime);

        if (currentState != previousState && move.magnitude > 0.1f)
        {
            Debug.Log("Player is now " + currentState.ToString().ToLower());
            previousState = currentState;
        }
    }

    void ApplyGravity()
    {
        // Ground check
        Vector3 groundCheckPos = transform.position + Vector3.down * (controller.height / 2 - controller.skinWidth);
        isGrounded = Physics.CheckSphere(groundCheckPos, groundCheckDistance, groundMask);

        // Reset falling velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouchToggle()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                controller.height = crouchHeight;
                playerCamera.localPosition += Vector3.up * cameraCrouchOffset;
                Debug.Log("Player crouched");
            }
            else
            {
                controller.height = originalHeight;
                playerCamera.localPosition -= Vector3.up * cameraCrouchOffset;
                Debug.Log("Player stood up");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (controller == null) return;

        Vector3 groundCheckPos = transform.position + Vector3.down * (controller.height / 2 - controller.skinWidth);
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheckPos, groundCheckDistance);
    }
}

public enum MovementState
{
    Walking,
    Sprinting,
    Crouching
}