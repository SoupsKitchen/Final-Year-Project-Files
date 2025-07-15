using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorHighlighter : MonoBehaviour
{
    public Image cursorImage;
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.green;
    public float grabDistance = 3f;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red); // Visualize ray in Scene view

            if (hit.collider.CompareTag("Grabbable"))
            {
                cursorImage.color = highlightColor;
                return;
            }
        }

        cursorImage.color = defaultColor;
    }
}