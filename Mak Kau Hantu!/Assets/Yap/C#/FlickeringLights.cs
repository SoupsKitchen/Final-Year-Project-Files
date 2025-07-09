using System.Collections;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    public Light targetLight;
    public float minTimeBetweenFlickers = 3f;
    public float maxTimeBetweenFlickers = 8f;
    public float flickerDuration = 1f;
    public float minFlickerIntensity = 0f;
    public float maxFlickerIntensity = 5f;
    public float stableIntensity = 5f;
    public float flickerSpeed = 0.05f; 

    private void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Wait for a random time before flickering
            float waitTime = Random.Range(minTimeBetweenFlickers, maxTimeBetweenFlickers);
            yield return new WaitForSeconds(waitTime);

            // Start flickering for a fixed duration
            float elapsed = 0f;
            while (elapsed < flickerDuration)
            {
                targetLight.intensity = Random.Range(minFlickerIntensity, maxFlickerIntensity);
                elapsed += flickerSpeed;
                yield return new WaitForSeconds(flickerSpeed);
            }

            // Restore to normal stable light
            targetLight.intensity = stableIntensity;
        }
    }
}
