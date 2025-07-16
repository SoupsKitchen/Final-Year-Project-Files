using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class FogTracking : MonoBehaviour
{
    public Transform player;
    public VisualEffect fogVFX;

    void Update()
    {
        if (fogVFX != null && player != null)
        {
            fogVFX.SetVector3("PlayerPosition", player.position);
        }
    }
}
