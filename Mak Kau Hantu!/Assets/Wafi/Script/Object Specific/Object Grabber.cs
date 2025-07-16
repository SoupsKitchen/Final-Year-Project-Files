using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public float minGrabDistance = 1f;
    public float maxGrabDistance = 10f;
    public float moveSpeed = 10f;
    public float scrollSpeed = 2f;

    private Rigidbody heldObject;
    private float currentGrabDistance = 3f;

    void Update()
    {
        HandleGrabInput();
        HandleHeldObjectMovement();
        HandleDistanceAdjustment();
    }

    void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, maxGrabDistance))
                {
                    if (hit.collider.CompareTag("Grabbable"))
                    {
                        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            heldObject = rb;
                            heldObject.useGravity = false;
                            heldObject.drag = 10f;

                            // Stop spinning
                            heldObject.angularVelocity = Vector3.zero;

                            // Prevent further spinning while held
                            heldObject.freezeRotation = true;

                            currentGrabDistance = Mathf.Clamp(hit.distance, minGrabDistance, maxGrabDistance);
                        }
                    }
                }
            }
            else
            {
                DropObject();
            }
        }
    }

    void HandleHeldObjectMovement()
    {
        if (heldObject != null)
        {
            Vector3 targetPos = Camera.main.transform.position + Camera.main.transform.forward * currentGrabDistance;
            Vector3 direction = targetPos - heldObject.position;
            heldObject.velocity = direction * moveSpeed;
        }
    }

    void HandleDistanceAdjustment()
    {
        if (heldObject != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            currentGrabDistance = Mathf.Clamp(currentGrabDistance + scroll * scrollSpeed, minGrabDistance, maxGrabDistance);
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.useGravity = true;
            heldObject.drag = 0f;

            // Allow rotation again
            heldObject.freezeRotation = false;

            heldObject = null;
        }
    }
}