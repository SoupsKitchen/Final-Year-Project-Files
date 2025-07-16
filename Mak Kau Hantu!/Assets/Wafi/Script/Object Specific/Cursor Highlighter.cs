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

        if (Physics.Raycast(ray, out hit, checkDistance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            if (hit.collider.CompareTag("Grabbable"))
            {
                cursorImage.color = highlightColor;
                return;
            }
        }

        cursorImage.color = defaultColor;
    }
}