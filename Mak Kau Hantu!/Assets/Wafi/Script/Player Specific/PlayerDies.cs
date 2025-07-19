using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDies : MonoBehaviour
{
    public GameObject deathScreen;
    void OnEnable()
    {
        FPSControllerCharacter FPS = GetComponent<FPSControllerCharacter>();
        if (FPS != null)
        {
            FPS.enabled = false;
        }
        else
        {
            Debug.LogWarning("FPS controller not found!");
        }

        if (deathScreen != null)
        {
            Instantiate(deathScreen);
        }
        else
        {
            Debug.LogWarning("The death screen prefab isn't added in!");
        }
        
    }
}
