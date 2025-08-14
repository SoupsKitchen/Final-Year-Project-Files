using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents FMODInstance { get; private set; }
    
    [field: Header("Ambience Sounds")]
    [field:SerializeField] public EventReference LevelAmbience {get; private set;}
    [field:SerializeField] public EventReference GhostAmbience {get; private set;}
    
    [field: Header("Ghost Sounds")]
    [field:SerializeField] public EventReference GhostHurt {get; private set;}
    [field:SerializeField] public EventReference VictoryLine {get; private set;}
    
    [field: Header("SFX Sounds")]
    [field:SerializeField] public EventReference DoorKnock {get; private set;}
    [field:SerializeField] public EventReference FlashLightClick {get; private set;}
    [field:SerializeField] public EventReference Footsteps {get; private set;}
    [field:SerializeField] public EventReference ItemPickup {get; private set;}
    
    [field: Header("UI Sounds")]
    [field:SerializeField] public EventReference ButtonCLick {get; private set;}
    [field:SerializeField] public EventReference PaperShuffling {get; private set;}
    
    private void Awake()
    {
        if (FMODInstance != null && FMODInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            FMODInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
