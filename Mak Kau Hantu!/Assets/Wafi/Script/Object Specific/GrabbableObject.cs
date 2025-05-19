using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class GrabbableObject : MonoBehaviour
{
    private Outline outline;
    private bool isHeld = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }

    public void SetHighlight(bool active)
    {
        if (outline != null && !isHeld)
        {
            outline.enabled = active;
            outline.color = 0; // 0 = green if configured that way
        }
    }

    public void SetHeld(bool held)
    {
        isHeld = held;
        if (outline != null)
        {
            outline.enabled = true;
            outline.color = held ? 1 : 0; // 1 = red, 0 = green
        }
    }
}
