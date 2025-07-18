using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContrroller : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    float xRotation = 0f;
    float previousSpeed = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        // Movement with Sprint
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        transform.position += move * currentSpeed * Time.deltaTime;

        // Log only when speed changes
        if (move.magnitude > 0f && currentSpeed != previousSpeed)
        {
            Debug.Log("Player Speed Changed: " + currentSpeed);
            previousSpeed = currentSpeed;
        }
    }
}