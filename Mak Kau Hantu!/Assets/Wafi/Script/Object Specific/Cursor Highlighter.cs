using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorHighlighter : MonoBehaviour
{
    public Image cursorImage;
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.green;

    public ObjectGrabber objectGrabber; // Drag your ObjectGrabber GameObject here

    void Update()
    {
        if (objectGrabber == null || cursorImage == null)
            return;

        float checkDistance = objectGrabber.maxGrabDistance;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Create a layer mask that ignores the "Player" layer
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, checkDistance, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            if (hit.collider.CompareTag("Grabbable") || hit.collider.CompareTag("Note"))
            {
                cursorImage.color = highlightColor;
                return;
            }
        }

        cursorImage.color = defaultColor;
    }
}