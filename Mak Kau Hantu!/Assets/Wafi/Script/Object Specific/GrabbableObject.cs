using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class GrabbableObject : MonoBehaviour
{
    private Outline outline;
    private Rigidbody rb;
    private bool isHeld = false;

    public float moveForce = 150f;
    public Transform holdPoint; // Assigned by grabber when picked up

    void Start()
    {
        outline = GetComponent<Outline>();
        rb = GetComponent<Rigidbody>();

        if (outline != null)
            outline.enabled = false;
    }

    void FixedUpdate()
    {
        if (isHeld && holdPoint != null)
        {
            Vector3 direction = holdPoint.position - transform.position;
            rb.AddForce(direction * moveForce, ForceMode.VelocityChange);
        }
    }

    public void SetHighlight(bool active)
    {
        if (outline != null && !isHeld)
        {
            outline.enabled = active;
            outline.color = 0; // 0 = green (hover)
        }
    }

    public void SetHeld(bool held, Transform newHoldPoint = null)
    {
        isHeld = held;

        if (outline != null)
        {
            outline.enabled = true;
            outline.color = held ? 1 : 0; // 1 = red (held), 0 = green (hover)
        }

        if (held)
        {
            rb.useGravity = false;
            rb.drag = 10f;
            rb.angularDrag = 10f;
            holdPoint = newHoldPoint;
        }
        else
        {
            rb.useGravity = true;
            rb.drag = 0;
            rb.angularDrag = 0.05f;
            rb.velocity = Vector3.zero;
            holdPoint = null;
        }
    }
}
