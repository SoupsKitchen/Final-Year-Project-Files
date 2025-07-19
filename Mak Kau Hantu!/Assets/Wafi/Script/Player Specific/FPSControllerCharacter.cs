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

    [Header("Circular Boundary")]
    public Vector3 boundaryCenter = Vector3.zero;
    public float boundaryRadius = 20f;

    private Vector3 velocity;
    private bool isGrounded;

    private CharacterController controller;
    private float xRotation = 0f;
    private MovementState currentState = MovementState.Walking;
    private MovementState previousState = MovementState.Walking;

    [Header("Pause")]
    public bool isPaused = false;

    [Header("Death")]
    private PlayerDies _pD;

    void Awake()
    {
        _pD = GetComponent<PlayerDies>();
        _pD.enabled = false;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
    }

    void Update()
    {
        if (isPaused)
            return;

        HandleLook();
        HandleCrouchToggle();
        HandleMovement();
        ApplyGravity();
        ClampToBoundary();
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
        Vector3 groundCheckPos = transform.position + Vector3.down * (controller.height / 2 - controller.skinWidth);
        isGrounded = Physics.CheckSphere(groundCheckPos, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ClampToBoundary()
    {
        Vector3 flatPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 flatCenter = new Vector3(boundaryCenter.x, 0f, boundaryCenter.z);
        Vector3 directionFromCenter = flatPosition - flatCenter;

        if (directionFromCenter.magnitude > boundaryRadius)
        {
            directionFromCenter = directionFromCenter.normalized * boundaryRadius;
            Vector3 clampedPosition = flatCenter + directionFromCenter;

            transform.position = new Vector3(clampedPosition.x, transform.position.y, clampedPosition.z);
        }
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(boundaryCenter.x, transform.position.y, boundaryCenter.z), boundaryRadius);

        if (controller == null) return;

        Vector3 groundCheckPos = transform.position + Vector3.down * (controller.height / 2 - controller.skinWidth);
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheckPos, groundCheckDistance);
    }

    public void Die()
    {
        _pD.enabled = true;
    }
}

public enum MovementState
{
    Walking,
    Sprinting,
    Crouching
}