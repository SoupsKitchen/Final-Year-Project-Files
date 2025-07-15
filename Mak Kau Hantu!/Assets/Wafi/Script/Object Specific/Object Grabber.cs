using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabber : MonoBehaviour
{
    public Transform holdPoint;
    public float grabDistance = 3f;
    public float moveSpeed = 10f;

    private Rigidbody heldObject;
    private GrabbableObject heldGrabbable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
                {
                    if (hit.collider.CompareTag("Grabbable"))
                    {
                        GrabObject(hit.rigidbody);
                    }
                }
            }
            else
            {
                DropObject();
            }
        }

        if (heldObject != null)
        {
            Vector3 targetPos = holdPoint.position;
            Vector3 direction = targetPos - heldObject.position;
            heldObject.velocity = direction * moveSpeed;
        }

        // Raycast every frame for highlight (only when not holding anything)
        if (heldObject == null)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, grabDistance))
            {
                if (hit.collider.CompareTag("Grabbable"))
                {
                    GrabbableObject grabbable = hit.collider.GetComponent<GrabbableObject>();
                    if (grabbable != null)
                        grabbable.SetHighlight(true);
                }
            }
        }

        void GrabObject(Rigidbody obj)
        {
            heldObject = obj;
            heldObject.useGravity = false;
            heldObject.drag = 10;

            heldGrabbable = obj.GetComponent<GrabbableObject>();
            if (heldGrabbable != null)
                heldGrabbable.SetHeld(true);
        }

        void DropObject()
        {
            heldObject.useGravity = true;
            heldObject.drag = 1;

            if (heldGrabbable != null)
            {
                heldGrabbable.SetHeld(false);
                heldGrabbable = null;
            }

            heldObject = null;
        }
    }
}


