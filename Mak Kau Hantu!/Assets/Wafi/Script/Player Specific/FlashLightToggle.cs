using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight;

    void Start()
    {
        if (flashlight != null)
            flashlight.enabled = false; // Start off
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlight != null)
                flashlight.enabled = !flashlight.enabled; // Toggle on/off
            AudioManager.AudioPlayer.PlayOneShot(FMODEvents.FMODInstance.FlashLightClick, transform.position);
        }
    }
}